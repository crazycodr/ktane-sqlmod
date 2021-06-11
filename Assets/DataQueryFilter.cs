/// <summary>
/// Represents a filter over a data row, encapsulates up to two column references or 1 column reference and a scalar value.
/// </summary>
public class DataQueryFilter
{
    /// <summary>
    /// Represents the left operand to perform filter on.
    /// </summary>
    public readonly DataRowColumnEnum leftOperandColumn;

    /// <summary>
    /// Represents the left operand to perform filter on when we have a value.
    /// </summary>
    public readonly int leftOperandValue = 0;

    /// <summary>
    /// Represents the left operand to perform filter on when we have a filter as an expression.
    /// </summary>
    public readonly DataQueryFilter leftOperandFilter = null;

    /// <summary>
    /// Represents the type of operand to work with on the left side.
    /// </summary>
    public readonly OperandTypeEnum leftOperandType = OperandTypeEnum.ValueOperand;

    /// <summary>
    /// Represents the right operand to perform filter on.
    /// </summary>
    public readonly DataRowColumnEnum rightOperandColumn;

    /// <summary>
    /// Represents the right operand to perform filter on when we have a value.
    /// </summary>
    public readonly int rightOperandValue = 0;

    /// <summary>
    /// Represents the right operand to perform filter on when we have a value.
    /// </summary>
    public readonly DataQueryFilter rightOperandFilter = null;

    /// <summary>
    /// Represents the type of operand to work with on the right side.
    /// </summary>
    public readonly OperandTypeEnum rightOperandType = OperandTypeEnum.ValueOperand;

    /// <summary>
    /// Represents the operator to apply on left and right operands.
    /// </summary>
    public readonly DataRowFilterOperatorEnum op = DataRowFilterOperatorEnum.OperatorEqual;

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
        // Resolve the left operand
        int leftOperand = 0;
        if (leftOperandType == OperandTypeEnum.ColumnOperand)
        {
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
            rightOperand = row.GetValueByColumn(leftOperandColumn);
        }
        else if (rightOperandType == OperandTypeEnum.FilterOperand)
        {
            rightOperand = leftOperandFilter.Apply(row) ? 1 : 0;
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

}