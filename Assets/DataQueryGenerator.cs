using System;
using System.Collections.Generic;

/// <summary>
/// Generates a data query for gameplay
/// </summary>
public class DataQueryGenerator
{
    /// <summary>
    /// Generates a simple query that uses only basic selections, filters and limits.
    /// </summary>
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

        // Prepare the selections between 2 and 3 columns at random, use PickRandom because "new Random.next(2, 3)" always yield 2
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
                result.filter = new DataQueryFilter(result.selections.PickRandom().column, possibleOperators.PickRandom(), new Random().Next(0, 9));
            }
            while (result.Apply(source).rows.Count == 0 && result.Apply(source).rows.Count == source.rows.Count);
        }
        else
        {
            // Set a filter and keep doing it as long as the result yields no data or the same data as before
            do
            {
                // Generate the left and right of the main filter
                DataQueryFilter filter1 = new DataQueryFilter(result.selections.PickRandom().column, possibleOperators.PickRandom(), new Random().Next(0, 9));
                DataQueryFilter filter2 = new DataQueryFilter(result.selections.PickRandom().column, possibleOperators.PickRandom(), new Random().Next(0, 9));

                // Generate the wrapping filter
                result.filter = new DataQueryFilter(filter1, possibleBooleanOperators.PickRandom(), filter2);
            }
            while (result.Apply(source).rows.Count == 0 && result.Apply(source).rows.Count == source.rows.Count);
        }

        // If the result yields 5 or more rows, skip 0-N/2 lines
        DataSet resultData = result.Apply(source);
        if (resultData.rows.Count >= 5)
        {
            result.limits.linesSkiped = new Random().Next(0, resultData.rows.Count / 2);
        }

        // If the result yields 4 or more rows, take 2-N lines
        resultData = result.Apply(source);
        if (resultData.rows.Count >= 4)
        {
            result.limits.linesTaken = new Random().Next(2, resultData.rows.Count);
        }

        // The resulting query is built
        return result;
    }
}