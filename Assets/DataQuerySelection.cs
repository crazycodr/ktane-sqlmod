/// <summary>
/// Represents a selection over a data query, encapsulates a column reference and a potential aggregate function.
/// </summary>
public class DataQuerySelection
{
    /// <summary>
    /// Represents the column to perform selection on.
    /// </summary>
    public DataRowColumnEnum column;

    /// <summary>
    /// Represents the filters applied on the data set. None by default.
    /// </summary>
    public DataQueryAggregatorEnum aggregator = DataQueryAggregatorEnum.None;

    /// <summary>
    /// Simple column selector
    /// </summary>
    /// <param name="column">Column to select</param>
    public DataQuerySelection(DataRowColumnEnum column)
    {
        this.column = column;
    }

    /// <summary>
    /// Aggregated column selector
    /// </summary>
    /// <param name="column">Column to select</param>
    /// <param name="aggregator">Aggregator to apply</param>
    public DataQuerySelection(DataRowColumnEnum column, DataQueryAggregatorEnum aggregator)
    {
        this.column = column;
        this.aggregator = aggregator;
    }

    public override string ToString()
    {
        switch (aggregator)
        {
            case DataQueryAggregatorEnum.Sum: return "SUM(" + ColumnEnumText(column) + ")";
            case DataQueryAggregatorEnum.Count: return "COUNT(" + ColumnEnumText(column) + ")";
            case DataQueryAggregatorEnum.Avg: return "AVG(" + ColumnEnumText(column) + ")";
            case DataQueryAggregatorEnum.Min: return "MIN(" + ColumnEnumText(column) + ")";
            case DataQueryAggregatorEnum.Max: return "MAX(" + ColumnEnumText(column) + ")";
            case DataQueryAggregatorEnum.None:
            default: return ColumnEnumText(column);
        }
    }

    /// <summary>
    /// Simply returns the next data row column value based on current.
    /// </summary>
    /// <param name="current">Current enum value</param>
    /// <returns>Next enum value</returns>
    private string ColumnEnumText(DataRowColumnEnum current)
    {
        switch (current)
        {
            case DataRowColumnEnum.ColumnA: return "A";
            case DataRowColumnEnum.ColumnB: return "B";
            case DataRowColumnEnum.ColumnC: return "C";
            case DataRowColumnEnum.ColumnD: return "D";
            case DataRowColumnEnum.ColumnE: return "E";
            case DataRowColumnEnum.ColumnF: return "F";
            case DataRowColumnEnum.ColumnG: return "G";
            case DataRowColumnEnum.None: return "-";
            default: return "-";
        }
    }

}