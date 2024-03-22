using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates a data query for gameplay
/// </summary>
public class DataQueryGenerator
{

    /// <summary>
    /// Calls the proper generate method based on difficulty.
    /// </summary>
    /// <param name="difficulty">The difficulty</param>
    /// <param name="source">The source of data to use to generate query</param>
    /// <returns>The generated data query</returns>
    public static DataQuery GenerateFromDifficulty(SqlModuleDifficultyEnum difficulty, DataSet source)
    {
        switch (difficulty)
        {
            case SqlModuleDifficultyEnum.Evil:
                return GenerateEvil(source);

            case SqlModuleDifficultyEnum.Cruel:
                return GenerateCruel(source);

            case SqlModuleDifficultyEnum.Basic:
            default:
                return GenerateSimple(source);
        }
    }

    /// <summary>
    /// Generates a simple query that uses only basic selections, filters and limits.
    /// </summary>
    /// <param name="source">The source of data to use to generate query</param>
    /// <returns>Generated data query</returns>
    public static DataQuery GenerateSimple(DataSet source)
    {
        DataQuery result = new DataQuery();

        // These are used to randomize the possible columns and filter operations
        DataRowColumnEnum[] possibleColumns = new DataRowColumnEnum[]
        {
            DataRowColumnEnum.ColumnA,
            DataRowColumnEnum.ColumnB,
            DataRowColumnEnum.ColumnC,
            DataRowColumnEnum.ColumnD,
            DataRowColumnEnum.ColumnE,
            DataRowColumnEnum.ColumnF,
            DataRowColumnEnum.ColumnG,
        };
        DataRowFilterOperatorEnum[] possibleOperators = new DataRowFilterOperatorEnum[]
        {
                DataRowFilterOperatorEnum.OperatorGreaterThan,
                DataRowFilterOperatorEnum.OperatorLessThan,
                DataRowFilterOperatorEnum.OperatorNotEqual,
                DataRowFilterOperatorEnum.OperatorEqual
        };
        DataRowFilterOperatorEnum[] possibleBooleanOperators = new DataRowFilterOperatorEnum[]
        {
                DataRowFilterOperatorEnum.OperatorAnd,
                DataRowFilterOperatorEnum.OperatorOr
        };
			
		// Prepare a random number of columns, use PickRandom because "new Random.next(2, 3)" always yield 2
		List<int> possibleColumnCounts = new List<int>() { 2, 3 };
		int numColumns = possibleColumnCounts.PickRandom();
		foreach (DataRowColumnEnum selectedColumn in possibleColumns.Shuffle().TakeLast(numColumns))
        {
            result.selections.Add(new DataQuerySelection(selectedColumn));
        }

		// Prepare a random number of filters, use PickRandom because "new Random.next(1, 2)" always yield 1
		List<int> possibleFilterCounts = new List<int>() { 1, 2 };
		int numRandomFilters = possibleFilterCounts.PickRandom();
        if (numRandomFilters == 1)
        {
            // Set a filter and keep doing it as long as the result yields no data
            do
            {
                result.filter = new DataQueryFilter(result.selections.PickRandom().column, possibleOperators.PickRandom(), new System.Random().Next(0, 9));
            }
            while (result.Apply(source).rows.Count < 2 || result.Apply(source).rows.Count == source.rows.Count);
        }
        else
        {
            // Set a filter and keep doing it as long as the result yields no data or the same data as before
            do
            {
                // Generate the left and right of the main filter
                DataQueryFilter filter1 = new DataQueryFilter(result.selections.PickRandom().column, possibleOperators.PickRandom(), new System.Random().Next(0, 9));
                DataQueryFilter filter2 = new DataQueryFilter(result.selections.PickRandom().column, possibleOperators.PickRandom(), new System.Random().Next(0, 9));

                // Generate the wrapping filter
                result.filter = new DataQueryFilter(filter1, possibleBooleanOperators.PickRandom(), filter2);

            }
            while (result.Apply(source).rows.Count < 2 || result.Apply(source).rows.Count == source.rows.Count);
        }

        // If the result yields 5 or more rows, skip 0-N/2 lines
        DataSet resultData = result.Apply(source);
        if (resultData.rows.Count >= 6)
        {
            result.limits.linesSkiped = new System.Random().Next(0, resultData.rows.Count / 2);
        }

        // If the result yields 4 or more rows, take 2-N lines
        resultData = result.Apply(source);
        if (resultData.rows.Count >= 4)
        {
            result.limits.linesTaken = new System.Random().Next(2, resultData.rows.Count);
        }

        // The resulting query is built
        return result;
    }

    /// <summary>
    /// Generates an evil query that uses aggregated selections and group by operations.
    /// </summary>
    /// <param name="source">The source of data to use to generate query</param>
    /// <returns>Generated data query</returns>
    public static DataQuery GenerateEvil(DataSet source)
    {
        DataQuery result = new DataQuery();

        // These are used to randomize the possible columns and aggregator operations
        DataRowColumnEnum[] possibleColumns = new DataRowColumnEnum[]
        {
            DataRowColumnEnum.ColumnA,
            DataRowColumnEnum.ColumnB,
            DataRowColumnEnum.ColumnC,
            DataRowColumnEnum.ColumnD,
            DataRowColumnEnum.ColumnE,
            DataRowColumnEnum.ColumnF,
            DataRowColumnEnum.ColumnG,
        };
        DataQueryAggregatorEnum[] possibleAggregators = new DataQueryAggregatorEnum[]
        {
            DataQueryAggregatorEnum.Avg,
            DataQueryAggregatorEnum.Count,
            DataQueryAggregatorEnum.Max,
            DataQueryAggregatorEnum.Min,
            DataQueryAggregatorEnum.Sum
        };

        // The selection should always be 3 columns in evil mode with 1 column grouped on (Non-aggregated)
        foreach (DataRowColumnEnum selectedColumn in possibleColumns.Shuffle().TakeLast(3))
        {
            result.selections.Add(new DataQuerySelection(selectedColumn));
        }

        // Apply an aggregator on each column
        result.selections[0].aggregator = possibleAggregators.PickRandom();
        result.selections[1].aggregator = possibleAggregators.PickRandom();
        result.selections[2].aggregator = possibleAggregators.PickRandom();

        // Revert a random column to be grouped on
        List<int> revertableIndexes = new List<int>() { 0, 1, 2 };
        int revertedIndex = revertableIndexes.PickRandom();
        result.selections[revertedIndex].aggregator = DataQueryAggregatorEnum.None;
        result.groupby.column = result.selections[revertedIndex].column;

        // The resulting query is built
        return result;
    }

    /// <summary>
    /// Generates a cruel query that uses aggregated selections, filters, groups and limits.
    /// </summary>
    /// <param name="source">The source of data to use to generate query</param>
    /// <returns>Generated data query</returns>
    public static DataQuery GenerateCruel(DataSet source)
    {
        DataQuery result = new DataQuery();

        // These are used to randomize the possible columns and aggregator operations
        DataRowColumnEnum[] possibleColumns = new DataRowColumnEnum[]
        {
            DataRowColumnEnum.ColumnA,
            DataRowColumnEnum.ColumnB,
            DataRowColumnEnum.ColumnC,
            DataRowColumnEnum.ColumnD,
            DataRowColumnEnum.ColumnE,
            DataRowColumnEnum.ColumnF,
            DataRowColumnEnum.ColumnG,
        };
        DataRowFilterOperatorEnum[] possibleOperators = new DataRowFilterOperatorEnum[]
        {
                DataRowFilterOperatorEnum.OperatorGreaterThan,
                DataRowFilterOperatorEnum.OperatorLessThan,
                DataRowFilterOperatorEnum.OperatorNotEqual,
                DataRowFilterOperatorEnum.OperatorEqual
        };
        DataQueryAggregatorEnum[] possibleAggregators = new DataQueryAggregatorEnum[]
        {
            DataQueryAggregatorEnum.Avg,
            DataQueryAggregatorEnum.Count,
            DataQueryAggregatorEnum.Max,
            DataQueryAggregatorEnum.Min,
            DataQueryAggregatorEnum.Sum
        };

        // The selection should always be 3 columns in evil mode with 1 column grouped on (Non-aggregated)
        foreach (DataRowColumnEnum selectedColumn in possibleColumns.Shuffle().TakeLast(3))
        {
            result.selections.Add(new DataQuerySelection(selectedColumn));
        }

        // Set a filter and group by and keep doing it as long as the result yields incorrect amount of data
        do
        {
            // Craft a filter
            result.filter = new DataQueryFilter(result.selections.PickRandom().column, possibleOperators.PickRandom(), new System.Random().Next(0, 9));

            // Apply an aggregator on each column
            result.selections[0].aggregator = possibleAggregators.PickRandom();
            result.selections[1].aggregator = possibleAggregators.PickRandom();
            result.selections[2].aggregator = possibleAggregators.PickRandom();

            // Revert a random column to be grouped on
            List<int> revertableIndexes = new List<int>() { 0, 1, 2 };
            int revertedIndex = revertableIndexes.PickRandom();
            result.selections[revertedIndex].aggregator = DataQueryAggregatorEnum.None;
            result.groupby.column = result.selections[revertedIndex].column;
        }
        while (result.Apply(source).rows.Count < 2 || result.Apply(source).rows.Count == source.rows.Count);

        // If the result yields 3 or more rows, skip 0 to 1 row
        DataSet resultData = result.Apply(source);
        if (resultData.rows.Count >= 3)
        {
            result.limits.linesSkiped = new System.Random().Next(0, 1);
        }

        // If the result yields 2 or more rows, take N to N-1 row
        resultData = result.Apply(source);
        if (resultData.rows.Count >= 3)
        {
            result.limits.linesTaken = new System.Random().Next(resultData.rows.Count - 1, resultData.rows.Count);
        }

        // The resulting query is built
        return result;
    }
}