/// <summary>
/// Represents the limitations applied to the results of the query.
/// </summary>
public class DataQueryLimitation
{
    /// <summary>
    /// Represents the number of lines to take from the results. 0 = everything
    /// </summary>
    public int linesTaken = 0;

    /// <summary>
    /// Represents the number of lines to skip from the results. 0 = none
    /// </summary>
    public int linesSkiped = 0;
}