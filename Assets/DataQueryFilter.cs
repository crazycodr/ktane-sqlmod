/// <summary>
/// Represents a filter over a data row, encapsulates up to two column references or 1 column reference and a scalar value.
/// </summary>
public class DataQueryFilter
{
    /// <summary>
    /// Represents the left operand to perform filter on.
    /// </summary>
    public DataRowColumnEnum leftOperandColumn;

    /// <summary>
    /// Represents the left operand to perform filter on when we have a value.
    /// </summary>
    public int leftOperandValue = 0;

    /// <summary>
    /// Represents the left operand to perform filter on when we have a filter as an expression.
    /// </summary>
    public DataQueryFilter leftOperandFilter = null;

    /// <summary>
    /// Represents the type of operand to work with on the left side.
    /// </summary>
    public OperandTypeEnum leftOperandType = OperandTypeEnum.ValueOperand;

    /// <summary>
    /// Represents the right operand to perform filter on.
    /// </summary>
    public DataRowColumnEnum rightOperandColumn;

    /// <summary>
    /// Represents the right operand to perform filter on when we have a value.
    /// </summary>
    public int rightOperandValue = 0;

    /// <summary>
    /// Represents the right operand to perform filter on when we have a value.
    /// </summary>
    public DataQueryFilter rightOperandFilter = null;

    /// <summary>
    /// Represents the type of operand to work with on the right side.
    /// </summary>
    public OperandTypeEnum rightOperandType = OperandTypeEnum.ValueOperand;

    /// <summary>
    /// Represents the operator to apply on left and right operands.
    /// </summary>
    public DataRowFilterOperatorEnum op = DataRowFilterOperatorEnum.OperatorEqual;

    /// <summary>
    /// Column to value version
    /// </summary>
    /// <param name="leftOperand">Left operand of filter</param>
    /// <param name="op">Operator to apply</param>
    /// <param name="rightOperand">Right operand of filter</param>
    public DataQueryFilter(DataRowColumnEnum leftOperand, DataRowFilterOperatorEnum op, int rightOperand)
    {
        leftOperandColumn = leftOperand;
        leftOperandType = OperandTypeEnum.ColumnOperand;
        this.op = op;
        rightOperandValue = rightOperand;
        rightOperandType = OperandTypeEnum.ValueOperand;
    }

    /// <summary>
    /// Column to column version
    /// </summary>
    /// <param name="leftOperand">Left operand of filter</param>
    /// <param name="op">Operator to apply</param>
    /// <param name="rightOperand">Right operand of filter</param>
    public DataQueryFilter(DataRowColumnEnum leftOperand, DataRowFilterOperatorEnum op, DataRowColumnEnum rightOperand)
    {
        leftOperandColumn = leftOperand;
        leftOperandType = OperandTypeEnum.ColumnOperand;
        this.op = op;
        rightOperandColumn = rightOperand;
        rightOperandType = OperandTypeEnum.ColumnOperand;
    }

    /// <summary>
    /// Column to filter version
    /// </summary>
    /// <param name="leftOperand">Left operand of filter</param>
    /// <param name="op">Operator to apply</param>
    /// <param name="rightOperand">Right operand of filter</param>
    public DataQueryFilter(DataRowColumnEnum leftOperand, DataRowFilterOperatorEnum op, DataQueryFilter rightOperand)
    {
        leftOperandColumn = leftOperand;
        leftOperandType = OperandTypeEnum.ColumnOperand;
        this.op = op;
        rightOperandFilter = rightOperand;
        rightOperandType = OperandTypeEnum.FilterOperand;
    }

    /// <summary>
    /// Value to value version
    /// </summary>
    /// <param name="leftOperand">Left operand of filter</param>
    /// <param name="op">Operator to apply</param>
    /// <param name="rightOperand">Right operand of filter</param>
    public DataQueryFilter(int leftOperand, DataRowFilterOperatorEnum op, int rightOperand)
    {
        leftOperandValue = leftOperand;
        leftOperandType = OperandTypeEnum.ValueOperand;
        this.op = op;
        rightOperandValue = rightOperand;
        rightOperandType = OperandTypeEnum.ValueOperand;
    }

    /// <summary>
    /// Value to column version
    /// </summary>
    /// <param name="leftOperand">Left operand of filter</param>
    /// <param name="op">Operator to apply</param>
    /// <param name="rightOperand">Right operand of filter</param>
    public DataQueryFilter(int leftOperand, DataRowFilterOperatorEnum op, DataRowColumnEnum rightOperand)
    {
        leftOperandValue = leftOperand;
        leftOperandType = OperandTypeEnum.ValueOperand;
        this.op = op;
        rightOperandColumn = rightOperand;
        rightOperandType = OperandTypeEnum.ColumnOperand;
    }

    /// <summary>
    /// Value to filter version
    /// </summary>
    /// <param name="leftOperand">Left operand of filter</param>
    /// <param name="op">Operator to apply</param>
    /// <param name="rightOperand">Right operand of filter</param>
    public DataQueryFilter(int leftOperand, DataRowFilterOperatorEnum op, DataQueryFilter rightOperand)
    {
        leftOperandValue = leftOperand;
        leftOperandType = OperandTypeEnum.ValueOperand;
        this.op = op;
        rightOperandFilter = rightOperand;
        rightOperandType = OperandTypeEnum.FilterOperand;
    }

    /// <summary>
    /// filter to value version
    /// </summary>
    /// <param name="leftOperand">Left operand of filter</param>
    /// <param name="op">Operator to apply</param>
    /// <param name="rightOperand">Right operand of filter</param>
    public DataQueryFilter(DataQueryFilter leftOperand, DataRowFilterOperatorEnum op, int rightOperand)
    {
        leftOperandFilter = leftOperand;
        leftOperandType = OperandTypeEnum.FilterOperand;
        this.op = op;
        rightOperandValue = rightOperand;
        rightOperandType = OperandTypeEnum.ValueOperand;
    }

    /// <summary>
    /// filter to column version
    /// </summary>
    /// <param name="leftOperand">Left operand of filter</param>
    /// <param name="op">Operator to apply</param>
    /// <param name="rightOperand">Right operand of filter</param>
    public DataQueryFilter(DataQueryFilter leftOperand, DataRowFilterOperatorEnum op, DataRowColumnEnum rightOperand)
    {
        leftOperandFilter = leftOperand;
        leftOperandType = OperandTypeEnum.FilterOperand;
        this.op = op;
        rightOperandColumn = rightOperand;
        rightOperandType = OperandTypeEnum.ColumnOperand;
    }

    /// <summary>
    /// filter to filter version
    /// </summary>
    /// <param name="leftOperand">Left operand of filter</param>
    /// <param name="op">Operator to apply</param>
    /// <param name="rightOperand">Right operand of filter</param>
    public DataQueryFilter(DataQueryFilter leftOperand, DataRowFilterOperatorEnum op, DataQueryFilter rightOperand)
    {
        leftOperandFilter = leftOperand;
        leftOperandType = OperandTypeEnum.FilterOperand;
        this.op = op;
        rightOperandFilter = rightOperand;
        rightOperandType = OperandTypeEnum.FilterOperand;
    }

    /// <summary>
    /// Applies the filter against a DataRow and returns positive or negative.
    /// </summary>
    /// <param name="row">The row to perform the filter operation over</param>
    /// <returns>A bool stating if row passes filter</returns>
    public bool Apply(DataRow row)
    {

        // If the op is none, just take the leftOperandFilter and short-circuit
        if (op == DataRowFilterOperatorEnum.OperatorNone)
        {
            return leftOperandFilter.Apply(row);
        }

        // Resolve the left operand
        int leftOperand = 0;
        if (leftOperandType == OperandTypeEnum.ColumnOperand)
        {
            if (leftOperandColumn == DataRowColumnEnum.None)
            {
                return false;
            }
            leftOperand = row.GetValueByColumn(leftOperandColumn);
        }
        else if (leftOperandType == OperandTypeEnum.FilterOperand)
        {
            leftOperand = leftOperandFilter.Apply(row) ? 1 : 0;
        }
        else
        {
            leftOperand = leftOperandValue;
        }

        // Resolve the right operand
        int rightOperand = 0;
        if (rightOperandType == OperandTypeEnum.ColumnOperand)
        {
            if (rightOperandColumn == DataRowColumnEnum.None)
            {
                return false;
            }
            rightOperand = row.GetValueByColumn(rightOperandColumn);
        }
        else if (rightOperandType == OperandTypeEnum.FilterOperand)
        {
            rightOperand = rightOperandFilter.Apply(row) ? 1 : 0;
        }
        else
        {
            rightOperand = rightOperandValue;
        }

        // Apply the operator
        switch (op)
        {
            case DataRowFilterOperatorEnum.OperatorAnd:
                return (leftOperand == 0 ? false : true) && (rightOperand == 0 ? false : true);
            case DataRowFilterOperatorEnum.OperatorEqual:
                return leftOperand == rightOperand;
            case DataRowFilterOperatorEnum.OperatorGreaterThan:
                return leftOperand > rightOperand;
            case DataRowFilterOperatorEnum.OperatorGreaterThanOrEqual:
                return leftOperand >= rightOperand;
            case DataRowFilterOperatorEnum.OperatorLessThan:
                return leftOperand < rightOperand;
            case DataRowFilterOperatorEnum.OperatorLessThanOrEqual:
                return leftOperand <= rightOperand;
            case DataRowFilterOperatorEnum.OperatorNotEqual:
                return leftOperand != rightOperand;
            case DataRowFilterOperatorEnum.OperatorOr:
                return (leftOperand == 0 ? false : true) || (rightOperand == 0 ? false : true);
        }

        return false;
    }

    public override string ToString()
    {

        // Short circuit to prevent none parent is to just get left operand and return it
        if (op == DataRowFilterOperatorEnum.OperatorNone)
        {
            return leftOperandFilter.ToString();
        }

        // In this case, build the parent and children completely
        string result = "";
        switch (leftOperandType)
        {
            case OperandTypeEnum.ColumnOperand: result += ColumnEnumText(leftOperandColumn); break;
            case OperandTypeEnum.FilterOperand: result += "(" + leftOperandFilter.ToString() + ")"; break;
            case OperandTypeEnum.ValueOperand: result += leftOperandValue; break;
            default: return "Unknown";
        }
        switch (op)
        {
            case DataRowFilterOperatorEnum.OperatorAnd: result += " AND "; break;
            case DataRowFilterOperatorEnum.OperatorOr: result += " OR "; break;
            case DataRowFilterOperatorEnum.OperatorGreaterThan: result += " > "; break;
            case DataRowFilterOperatorEnum.OperatorGreaterThanOrEqual: result += " >= "; break;
            case DataRowFilterOperatorEnum.OperatorLessThan: result += " < "; break;
            case DataRowFilterOperatorEnum.OperatorLessThanOrEqual: result += " <= "; break;
            case DataRowFilterOperatorEnum.OperatorNotEqual: result += " <> "; break;
            case DataRowFilterOperatorEnum.OperatorEqual: result += " = "; break;
            case DataRowFilterOperatorEnum.OperatorNone: result += " N/A "; break;
        }
        switch (rightOperandType)
        {
            case OperandTypeEnum.ColumnOperand: result += ColumnEnumText(rightOperandColumn); break;
            case OperandTypeEnum.FilterOperand: result += "(" + rightOperandFilter.ToString() + ")"; break;
            case OperandTypeEnum.ValueOperand: result += rightOperandValue; break;
            default: return "Unknown";
        }
        return result;
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