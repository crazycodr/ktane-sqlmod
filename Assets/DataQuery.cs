using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a query over a DataSet.
/// </summary>
public class DataQuery
{
    /// <summary>
    /// Represents the selection expressions the query will do on the data set.
    /// </summary>
    public readonly List<DataQuerySelection> selections = new List<DataQuerySelection>();

    /// <summary>
    /// Represents the filters applied on the data set, this is only one filter but the filter can be a boolean filter of other filters.
    /// </summary>
    /// <remarks>Remember that filters already implement the concept of parenthesis so you have to build the and/or combinations yourself properly.</remarks>
    public DataQueryFilter filter = null;

    /// <summary>
    /// Represents the group on the data set.
    /// </summary>
    public readonly DataQueryGroup group = new DataQueryGroup();

    /// <summary>
    /// Represents the limitations on the result set.
    /// </summary>
    public readonly DataQueryLimitation limits = new DataQueryLimitation();

    /// <summary>
    /// Applies the query to a dataset
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public DataSet Apply(DataSet source)
    {
        // Create a new data set from valid rows
        DataSet result = new DataSet();
        result.rows = source.rows.Where(row => filter == null || filter.Apply(row)).ToList();

        // Now group and aggregate the results or just select the end results
        if (group.column != DataRowNoneColumnEnum.None)
        {
            // Ensure that all selections are either aggregated or are the column grouped on
            if (selections.Where(s =>
            {
                return s.aggregator != DataQueryAggregatorEnum.None || (int)s.column == (int)group.column;
            }).Count() != selections.Count())
            {
                throw new System.InvalidOperationException("Must aggregate or group on each selection when using a group");
            }

            // Create the groups
            IEnumerable<IGrouping<int, DataRow>> groupedResults = result.rows.GroupBy(row => row.GetValueByColumn(group.column), row => row);

            // For each group, apply the aggregators on a new DataRow and add it to the result (after clearing all rows from it)
            result.rows.Clear();
            foreach (IGrouping<int, DataRow> grouping in groupedResults)
            {
                DataRow resultingRow = new DataRow();
                foreach (DataQuerySelection selection in selections)
                {
                    switch (selection.aggregator)
                    {
                        case DataQueryAggregatorEnum.Avg:
                            resultingRow.SetValueByColumn(selection.column, (int)Math.Round(grouping.Average(row => row.GetValueByColumn(selection.column)), 0));
                            break;
                        case DataQueryAggregatorEnum.Count:
                            resultingRow.SetValueByColumn(selection.column, grouping.Count());
                            break;
                        case DataQueryAggregatorEnum.Max:
                            resultingRow.SetValueByColumn(selection.column, grouping.Max(row => row.GetValueByColumn(selection.column)));
                            break;
                        case DataQueryAggregatorEnum.Min:
                            resultingRow.SetValueByColumn(selection.column, grouping.Min(row => row.GetValueByColumn(selection.column)));
                            break;
                        case DataQueryAggregatorEnum.Sum:
                            resultingRow.SetValueByColumn(selection.column, grouping.Sum(row => row.GetValueByColumn(selection.column)));
                            break;
                        case DataQueryAggregatorEnum.None:
                        default:
                            resultingRow.SetValueByColumn(selection.column, grouping.Key);
                            break;
                    }
                }
                result.rows.Add(resultingRow);
            }
        }
        else
        {
            // Ensure that all selections are non aggregated
            if (selections.Where(s =>
            {
                return s.aggregator == DataQueryAggregatorEnum.None;
            }).Count() != selections.Count())
            {
                throw new InvalidOperationException("Must not aggregate on any selection when not using a group");
            }

            // Take only the selected fields
            for (int iRow = 0; iRow < result.rows.Count(); iRow++)
            {
                DataRow resultingRow = new DataRow();
                foreach (DataQuerySelection selection in selections)
                {
                    resultingRow.SetValueByColumn(selection.column, result.rows[iRow].GetValueByColumn(selection.column));
                }
                result.rows[iRow] = resultingRow;
            }

        }

        // Apply limits and return the result
        result.rows = result.rows.Skip(limits.linesSkiped).Take(limits.linesTaken).ToList();
        return result;

    }

}