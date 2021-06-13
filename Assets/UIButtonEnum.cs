/// <summary>
/// Represents all buttons user can press. Directly used by the delegate to simplify the number of potential button delegates
/// or we'd have tons of delegates for similar methods.
/// </summary>
public enum UIButtonEnum
{
    Selection1,
    Selection2,
    Selection3,
    Selection1Group,
    Selection2Group,
    Selection3Group,
    Where1Left,
    Where1Op,
    Where1Right,
    WhereOp,
    Where2Left,
    Where2Op,
    Where2Right,
    LimitTake,
    LimitSkip
}