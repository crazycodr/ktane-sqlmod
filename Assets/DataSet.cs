using System.Collections.Generic;

/// <summary>
/// Represents a dataset that the module will use either as a datasource or an expected result.
/// 
/// The DataSet is functional with Linq which is how we'll create the expected DataSet.
/// </summary>
public class DataSet
{

    /// <summary>
    /// Contains all rows in the dataset
    /// </summary>
    public IList<DataRow> rows = new List<DataRow>();

    /// <summary>
    /// Adds a row to the dataset
    /// </summary>
    /// <param name="row">Row of data to add to the dataset</param>
    public void AddRow(DataRow row)
    {
        rows.Add(row);
    }

    /// <summary>
    /// Compares this dataset and another dataset for equality. Equality is when all rows of both datasets are found on both sides and are completely equal.
    /// </summary>
    /// <param name="other">The other dataset to compare against</param>
    /// <returns>Comparison equality</returns>
    public bool CompareTo(DataSet other)
    {
        if (rows.Count != other.rows.Count)
        {
            return false;
        }
        for (int i = 0; i < rows.Count; i++)
        {
            if (!rows[i].CompareTo(other.rows[i]))
            {
                return false;
            }
        }
        return true;
    }
}