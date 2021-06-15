using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a row of data in a DataSet.
/// </summary>
public class DataRow
{
    /// <summary>
    /// Values for the datarow
    /// </summary>
    protected readonly IList<int> values = new List<int>();

    /// <summary>
    /// Constructor for the DataRow, initializes everything to 0
    /// </summary>
    public DataRow()
    {
        for (int col = (int)DataRowColumnEnum.ColumnA; col <= (int)DataRowColumnEnum.ColumnG; col++)
        {
            values.Add(0);
        }
    }

    /// <summary>
    /// Constructor for the DataRow, accepts values that the SQL Module will play with.
    /// </summary>
    /// <param name="values">Values for the row, values must be provided in the same order as they must match as exposed columns.</param>
    public DataRow(IList<int> values)
    {
        if (values.Count() < (int)DataRowColumnEnum.ColumnG)
        {
            throw new System.FormatException("You must provide all columns to be contained in the DataRow up to a max of " + (int)DataRowColumnEnum.ColumnG);
        }
        this.values = values.ToList();
    }

    /// <summary>
    /// Returns the value matching the requested column.
    /// </summary>
    /// <param name="column">The column to retrieve.</param>
    /// <returns>The value associated to that column in the row.</returns>
    public int GetValueByColumn(DataRowColumnEnum column)
    {
        return values[(int)column];
    }

    /// <summary>
    /// Sets the values of the column to a certain value
    /// </summary>
    /// <param name="column">The column to set.</param>
    /// <param name="value">The values to set.</param>
    public void SetValueByColumn(DataRowColumnEnum column, int value)
    {
        values[(int)column] = value;
    }

    /// <summary>
    /// Compares two rows together index by index.
    /// </summary>
    /// <param name="other">The other row to compare against</param>
    /// <returns>Returns true if both rows are perfectly equal</returns>
    public bool CompareTo(DataRow other)
    {
        for(int i = (int)DataRowColumnEnum.ColumnA; i < (int)DataRowColumnEnum.ColumnG; i++)
        {
            if (other.values[i] != values[i])
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Returns the value, as a string of a DataRow by joining all values in it together
    /// </summary>
    public override string ToString()
    {
        return "[" + values.Join(",") + "]";
    }
}