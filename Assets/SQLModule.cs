using KeepCoding;
using System;
using UnityEngine;

public class SQLModule : ModuleScript
{
    /// <summary>
    /// Data set source used by the module
    /// </summary>
    DataSet source;

    /// <summary>
    /// Data set goal to achieve
    /// </summary>
    DataSet goal;

    /// <summary>
    /// Query the user is building through the UI
    /// </summary>
    DataQuery query;

    /// <summary>
    /// Defines if the module is in editorMode or goalMode
    /// </summary>
    bool isEditorMode = true;

    /// <summary>
    /// Used as a  caching for KMSelectable component of self
    /// </summary>
    private KMSelectable selectableSelf;

    /// <summary>
    /// Used as a caching for TextMesh of a button.
    /// </summary>
    private TextMesh selection1Label;

    /// <summary>
    /// Used as a caching for TextMesh of a button.
    /// </summary>
    private TextMesh selection2Label;

    /// <summary>
    /// Used as a caching for TextMesh of a button.
    /// </summary>
    private TextMesh selection3Label;

    /// <summary>
    /// Used as a caching for TextMesh of a button.
    /// </summary>
    private TextMesh whereCombinationOperatorLabel;

    /// <summary>
    /// Used as a caching for TextMesh of a button.
    /// </summary>
    private TextMesh where1LeftOperandLabel;

    /// <summary>
    /// Used as a caching for TextMesh of a button.
    /// </summary>
    private TextMesh where1OperatorLabel;

    /// <summary>
    /// Used as a caching for TextMesh of a button.
    /// </summary>
    private TextMesh where1RightOperandLabel;

    /// <summary>
    /// Used as a caching for TextMesh of a button.
    /// </summary>
    private TextMesh where2LeftOperandLabel;

    /// <summary>
    /// Used as a caching for TextMesh of a button.
    /// </summary>
    private TextMesh where2OperatorLabel;

    /// <summary>
    /// Used as a caching for TextMesh of a button.
    /// </summary>
    private TextMesh where2RightOperandLabel;

    /// <summary>
    /// Used as a caching for TextMesh of a button.
    /// </summary>
    private TextMesh limitSkipButtonLabel;

    /// <summary>
    /// Used as a caching for TextMesh of a button.
    /// </summary>
    private TextMesh limitTakeButtonLabel;

    /// <summary>
    /// Button used to switch from goal to editor
    /// </summary>
    public KMSelectable modeButton;

    /// <summary>
    /// Button used to trigger a check
    /// </summary>
    public KMSelectable checkButton;

    /// <summary>
    /// Label used for selections
    /// </summary>
    public TextMesh selectionLabel;

    /// <summary>
    /// Button used to manage selection 1
    /// </summary>
    public KMSelectable selection1Button;

    /// <summary>
    /// Button used to manage selection 2
    /// </summary>
    public KMSelectable selection2Button;

    /// <summary>
    /// Button used to manage selection 3
    /// </summary>
    public KMSelectable selection3Button;

    /// <summary>
    /// Button used to manage selection 1 grouping
    /// </summary>
    public KMSelectable selection1GroupButton;

    /// <summary>
    /// Button used to manage selection 2 grouping
    /// </summary>
    public KMSelectable selection2GroupButton;

    /// <summary>
    /// Button used to manage selection 3 grouping
    /// </summary>
    public KMSelectable selection3GroupButton;

    /// <summary>
    /// Label used for filters
    /// </summary>
    public TextMesh whereLabel;

    /// <summary>
    /// Button used to manage where 1 left operand
    /// </summary>
    public KMSelectable where1LeftOperandButton;

    /// <summary>
    /// Button used to manage where 1 operator
    /// </summary>
    public KMSelectable where1OperatorButton;

    /// <summary>
    /// Button used to manage where 1 right operand
    /// </summary>
    public KMSelectable where1RightOperandButton;

    /// <summary>
    /// Button used to manage where combination operator
    /// </summary>
    public KMSelectable whereCombinationOperatorButton;

    /// <summary>
    /// Button used to manage where 2 left operand
    /// </summary>
    public KMSelectable where2LeftOperandButton;

    /// <summary>
    /// Button used to manage where 2 operator
    /// </summary>
    public KMSelectable where2OperatorButton;

    /// <summary>
    /// Button used to manage where 2 right operand
    /// </summary>
    public KMSelectable where2RightOperandButton;

    /// <summary>
    /// Used as a caching for TextMesh label.
    /// </summary>
    public TextMesh limitSkipLabel;

    /// <summary>
    /// Used as a caching for TextMesh label.
    /// </summary>
    public TextMesh limitTakeLabel;

    /// <summary>
    /// Button used to manage taken rows
    /// </summary>
    public KMSelectable limitTakeButton;

    /// <summary>
    /// Button used to manage skipped rows
    /// </summary>
    public KMSelectable limitSkipButton;

    /// <summary>
    /// An array that contains all goal game objects, they must contain a TextMesh and are expected to be to the count of 15 (5 x 3)
    /// </summary>
    public GameObject[] goalLabels;

    /// <summary>
    /// Sets up the whole module
    /// </summary>
    void Start()
    {
        InitDataObjects();
        InitUI();
        UpdateUI();
    }

    /// <summary>
    /// Prepares the data objects for this module.
    /// </summary>
    void InitDataObjects()
    {

        // For now, use the 1st and only dataset matrix #1
        Log("Data source set to 1");
        source = DataSetFactory.FromIntMatrix(DataSetFactory.dataSetMatrix1);
        Log("Source set to: " + source.ToString());

        // Generate a goal
        Log("Generating target query");
        DataQuery targetQuery = DataQueryGenerator.GenerateSimple(source);
        Log("Target query is: " + targetQuery.ToString());
        goal = targetQuery.Apply(source);

        // Push the data of the goal into the goal labels
        int goalLabelIndex = 0;
        foreach (GameObject goalLabel in goalLabels)
        {
            int goalRow = (int)Math.Floor((double)goalLabelIndex / 3);
            int goalCol = goalLabelIndex % 3;
            goalLabel.GetComponent<TextMesh>().text = goal.rows.Count > goalRow && targetQuery.selections.Count > goalCol ? goal.rows[goalRow].GetValueByColumn((DataRowColumnEnum)goalCol).ToString() : "";
            goalLabelIndex++;
        }

        // Prepare the base query for the UI
        query = new DataQuery();
        query.selections.Add(new DataQuerySelection(DataRowColumnEnum.ColumnA));
        query.selections.Add(new DataQuerySelection(DataRowColumnEnum.ColumnB));
        query.selections.Add(new DataQuerySelection(DataRowColumnEnum.ColumnC));
        query.filter = new DataQueryFilter(
            new DataQueryFilter(DataRowColumnEnum.ColumnA, DataRowFilterOperatorEnum.OperatorEqual, 0),
            DataRowFilterOperatorEnum.OperatorNone,
            new DataQueryFilter(DataRowColumnEnum.ColumnA, DataRowFilterOperatorEnum.OperatorEqual, 0)
        );

    }

    /// <summary>
    /// Prepare the initial UI
    /// </summary>
    void InitUI()
    {
        // Editor buttons, some are optional based on version of module
        selection1Button.Assign(onInteract: () => OnPress(UIButtonEnum.Selection1));
        selection2Button.Assign(onInteract: () => OnPress(UIButtonEnum.Selection2));
        selection3Button.Assign(onInteract: () => OnPress(UIButtonEnum.Selection3));
        if (selection1GroupButton) selection1GroupButton.Assign(onInteract: () => OnPress(UIButtonEnum.Selection1Group));
        if (selection2GroupButton) selection2GroupButton.Assign(onInteract: () => OnPress(UIButtonEnum.Selection2Group));
        if (selection3GroupButton) selection3GroupButton.Assign(onInteract: () => OnPress(UIButtonEnum.Selection3Group));
        where1LeftOperandButton.Assign(onInteract: () => OnPress(UIButtonEnum.Where1Left));
        where1OperatorButton.Assign(onInteract: () => OnPress(UIButtonEnum.Where1Op));
        where1RightOperandButton.Assign(onInteract: () => OnPress(UIButtonEnum.Where1Right));
        where2LeftOperandButton.Assign(onInteract: () => OnPress(UIButtonEnum.Where2Left));
        where2OperatorButton.Assign(onInteract: () => OnPress(UIButtonEnum.Where2Op));
        where2RightOperandButton.Assign(onInteract: () => OnPress(UIButtonEnum.Where2Right));
        whereCombinationOperatorButton.Assign(onInteract: () => OnPress(UIButtonEnum.WhereOp));
        limitTakeButton.Assign(onInteract: () => OnPress(UIButtonEnum.LimitTake));
        limitSkipButton.Assign(onInteract: () => OnPress(UIButtonEnum.LimitSkip));

        // Acquire reference to TextMeshes for better updates
        selection1Label = selection1Button.GetComponentInChildren<TextMesh>();
        selection2Label = selection2Button.GetComponentInChildren<TextMesh>();
        selection3Label = selection3Button.GetComponentInChildren<TextMesh>();
        whereCombinationOperatorLabel = whereCombinationOperatorButton.GetComponentInChildren<TextMesh>();
        where1LeftOperandLabel = where1LeftOperandButton.GetComponentInChildren<TextMesh>();
        where1OperatorLabel = where1OperatorButton.GetComponentInChildren<TextMesh>();
        where1RightOperandLabel = where1RightOperandButton.GetComponentInChildren<TextMesh>();
        where2LeftOperandLabel = where2LeftOperandButton.GetComponentInChildren<TextMesh>();
        where2OperatorLabel = where2OperatorButton.GetComponentInChildren<TextMesh>();
        where2RightOperandLabel = where2RightOperandButton.GetComponentInChildren<TextMesh>();
        limitSkipButtonLabel = limitSkipButton.GetComponentInChildren<TextMesh>();
        limitTakeButtonLabel = limitTakeButton.GetComponentInChildren<TextMesh>();

        // Mode button
        modeButton.OnInteract += OnModeChange;

        // Check button
        checkButton.OnInteract += OnCheck;

        // Acquire selectableSelf for later use
        selectableSelf = GetComponent<KMSelectable>();
    }

    /// <summary>
    /// Checks if the module has been properly answered
    /// </summary>
    /// <returns>Not sure?</returns>
    bool OnCheck()
    {

        // Only do something if module is activated and enabled
        if (!isActiveAndEnabled && !IsSolved)
        {
            return false;
        }

        // Only take input if not solved
        if (IsSolved)
        {
            return false;
        }

        // Play a sound on click
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);

        // Generate a result from query and source
        Log("Applying query on source: " + query.ToString());
        DataSet result = query.Apply(source);
        Log("Result is: " + result.ToString());

        // Compare result and goal and strike or solve
        Log("Testing against goal: " + goal.ToString());
        if (goal.ToString() == result.ToString())
        {
            Solve("Module disarmed");
        }
        else
        {
            Strike("Module striked due to dataset differences");
        }

        // Update the UI once this has hapenned
        UpdateUI();

        return false;
    }

    /// <summary>
    /// Changes the view mode of the module
    /// </summary>
    /// <returns>Not sure?</returns>
    bool OnModeChange()
    {

        // Only do something if module is activated and enabled
        if (!isActiveAndEnabled)
        {
            return false;
        }

        // Play a sound on click
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);

        // Switch the mode
        isEditorMode = !isEditorMode;

        // Update the button label
        modeButton.GetComponentInChildren<TextMesh>().text = isEditorMode ? "Goal" : "Editor";

        // Hide/Show all applicable items for the editor
        // Start with selection components
        selectionLabel.gameObject.SetActive(isEditorMode);
        selection1Button.gameObject.SetActive(isEditorMode);
        if (selection1GroupButton) selection1GroupButton.gameObject.SetActive(isEditorMode);
        selection2Button.gameObject.SetActive(isEditorMode);
        if (selection2GroupButton) selection2GroupButton.gameObject.SetActive(isEditorMode);
        selection3Button.gameObject.SetActive(isEditorMode);
        if (selection3GroupButton) selection3GroupButton.gameObject.SetActive(isEditorMode);

        // Next, the filter components
        whereLabel.gameObject.SetActive(isEditorMode);
        where1LeftOperandButton.gameObject.SetActive(isEditorMode);
        where1OperatorButton.gameObject.SetActive(isEditorMode);
        where1RightOperandButton.gameObject.SetActive(isEditorMode);
        where2LeftOperandButton.gameObject.SetActive(isEditorMode);
        where2OperatorButton.gameObject.SetActive(isEditorMode);
        where2RightOperandButton.gameObject.SetActive(isEditorMode);
        whereCombinationOperatorButton.gameObject.SetActive(isEditorMode);

        // Finaly, the limitation components
        limitSkipLabel.gameObject.SetActive(isEditorMode);
        limitSkipButton.gameObject.SetActive(isEditorMode);
        limitTakeLabel.gameObject.SetActive(isEditorMode);
        limitTakeButton.gameObject.SetActive(isEditorMode);

        // Hide/Show all applicable items for the goal
        foreach (GameObject goalLabel in goalLabels)
        {
            goalLabel.gameObject.SetActive(!isEditorMode);
        }

        // Update the ui to ensure the where filters are properly enabled, we don't want to repeat the logic here
        if (isEditorMode)
        {
            UpdateUI();
        }

        return false;
    }

    /// <summary>
    /// Used to handle presses of different game buttons. To simplify the number of delegates, we'll
    /// have a limited number of delegates and group buttons in similar delegates.
    /// </summary>
    /// <param name="buttonPressed">Button pressed</param>
    void OnPress(UIButtonEnum buttonPressed)
    {

        // Only do something if module is activated and enabled
        if (!isActiveAndEnabled)
        {
            return;
        }

        // Only take input if not solved
        if (IsSolved)
        {
            return;
        }

        // Play a sound on click
        PlaySound(KMSoundOverride.SoundEffect.SelectionTick);
        
        // Process the button click
        switch (buttonPressed)
        {
            case UIButtonEnum.Selection1:
                query.selections[0].column = NextColumnEnum(query.selections[0].column);
                break;
            case UIButtonEnum.Selection2:
                query.selections[1].column = NextColumnEnum(query.selections[1].column);
                break;
            case UIButtonEnum.Selection3:
                query.selections[2].column = NextColumnEnum(query.selections[2].column);
                break;
            case UIButtonEnum.Selection1Group:
                query.selections[0].aggregator = NextAggregatorEnum(query.selections[0].aggregator);
                break;
            case UIButtonEnum.Selection2Group:
                query.selections[1].aggregator = NextAggregatorEnum(query.selections[1].aggregator);
                break;
            case UIButtonEnum.Selection3Group:
                query.selections[2].aggregator = NextAggregatorEnum(query.selections[2].aggregator);
                break;
            case UIButtonEnum.Where1Left:
                query.filter.leftOperandFilter.leftOperandColumn = NextFilterColumnEnum(query.filter.leftOperandFilter.leftOperandColumn);
                break;
            case UIButtonEnum.Where1Op:
                query.filter.leftOperandFilter.op = NextFilterEnum(query.filter.leftOperandFilter.op);
                break;
            case UIButtonEnum.Where1Right:
                query.filter.leftOperandFilter.rightOperandValue++;
                if (query.filter.leftOperandFilter.rightOperandValue > 9)
                {
                    query.filter.leftOperandFilter.rightOperandValue = 0;
                }
                break;
            case UIButtonEnum.Where2Left:
                query.filter.rightOperandFilter.leftOperandColumn = NextFilterColumnEnum(query.filter.rightOperandFilter.leftOperandColumn);
                break;
            case UIButtonEnum.Where2Op:
                query.filter.rightOperandFilter.op = NextFilterEnum(query.filter.rightOperandFilter.op);
                break;
            case UIButtonEnum.Where2Right:
                query.filter.rightOperandFilter.rightOperandValue++;
                if (query.filter.rightOperandFilter.rightOperandValue > 9)
                {
                    query.filter.rightOperandFilter.rightOperandValue = 0;
                }
                break;
            case UIButtonEnum.WhereOp:
                query.filter.op = NextCombinationFilterEnum(query.filter.op);
                break;
            case UIButtonEnum.LimitSkip:
                query.limits.linesSkiped += 1;
                if (query.limits.linesSkiped > 9)
                {
                    query.limits.linesSkiped = 0;
                }
                break;
            case UIButtonEnum.LimitTake:
                query.limits.linesTaken += 1;
                if (query.limits.linesTaken > 9)
                {
                    query.limits.linesTaken = 0;
                }
                break;
        }

        // Update the UI
        UpdateUI();

    }

    /// <summary>
    /// Updates the UI to the correct state
    /// </summary>
    private void UpdateUI()
    {

        // Selection 1 setup
        selection1Label.text = ColumnEnumText(query.selections[0].column);
        if (selection1GroupButton) selection1GroupButton.gameObject.SetActive(true);

        // Selection 2 setup
        selection2Label.text = ColumnEnumText(query.selections[1].column);
        if (selection2GroupButton) selection2GroupButton.gameObject.SetActive(true);

        // Selection 3 setup
        selection3Label.text = ColumnEnumText(query.selections[2].column);
        if (selection3GroupButton) selection3GroupButton.gameObject.SetActive(true);

        // Show or hide the second filter, adjust selectableSelf children as needed
        where2LeftOperandButton.gameObject.SetActive(query.filter.op != DataRowFilterOperatorEnum.OperatorNone);
        where2OperatorButton.gameObject.SetActive(query.filter.op != DataRowFilterOperatorEnum.OperatorNone);
        where2RightOperandButton.gameObject.SetActive(query.filter.op != DataRowFilterOperatorEnum.OperatorNone);
        if (query.filter.op != DataRowFilterOperatorEnum.OperatorNone)
        {
            selectableSelf.Children[12] = whereCombinationOperatorButton;
            selectableSelf.Children[13] = where2LeftOperandButton;
            selectableSelf.Children[14] = where2OperatorButton;
            selectableSelf.Children[15] = where2RightOperandButton;
            selectableSelf.UpdateChildren();
        }
        else
        {
            selectableSelf.Children[12] = whereCombinationOperatorButton;
            selectableSelf.Children[13] = null;
            selectableSelf.Children[14] = null;
            selectableSelf.Children[15] = null;
            selectableSelf.UpdateChildren();
        }

        // Update the where combination label
        whereCombinationOperatorLabel.text = FilterEnumText(query.filter.op);

        // Adjust the filter 1 data
        where1LeftOperandLabel.text = ColumnEnumText(query.filter.leftOperandFilter.leftOperandColumn);
        where1OperatorLabel.text = FilterEnumText(query.filter.leftOperandFilter.op);
        where1RightOperandLabel.text = query.filter.leftOperandFilter.rightOperandValue.ToString();

        // Adjust the filter 2 data
        where2LeftOperandLabel.text = ColumnEnumText(query.filter.rightOperandFilter.leftOperandColumn);
        where2OperatorLabel.text = FilterEnumText(query.filter.rightOperandFilter.op);
        where2RightOperandLabel.text = query.filter.rightOperandFilter.rightOperandValue.ToString();

        // Adjust the limit labels
        limitSkipButtonLabel.text = query.limits.linesSkiped == 0 ? "None" : query.limits.linesSkiped.ToString();
        limitTakeButtonLabel.text = query.limits.linesTaken == 0 ? "All" : query.limits.linesTaken.ToString();
    }

    /// <summary>
    /// Simply returns the next data row column value based on current.
    /// </summary>
    /// <param name="current">Current enum value</param>
    /// <returns>Next enum value</returns>
    private DataRowColumnEnum NextColumnEnum(DataRowColumnEnum current)
    {
        switch (current)
        {
            case DataRowColumnEnum.None: return DataRowColumnEnum.ColumnA;
            case DataRowColumnEnum.ColumnA: return DataRowColumnEnum.ColumnB;
            case DataRowColumnEnum.ColumnB: return DataRowColumnEnum.ColumnC;
            case DataRowColumnEnum.ColumnC: return DataRowColumnEnum.ColumnD;
            case DataRowColumnEnum.ColumnD: return DataRowColumnEnum.ColumnE;
            case DataRowColumnEnum.ColumnE: return DataRowColumnEnum.ColumnF;
            case DataRowColumnEnum.ColumnF: return DataRowColumnEnum.ColumnG;
            case DataRowColumnEnum.ColumnG: return DataRowColumnEnum.None;
            default: return DataRowColumnEnum.None;
        }
    }

    /// <summary>
    /// Simply returns the next data row column value based on current.
    /// </summary>
    /// <param name="current">Current enum value</param>
    /// <returns>Next enum value</returns>
    private DataRowColumnEnum NextFilterColumnEnum(DataRowColumnEnum current)
    {
        switch (current)
        {
            case DataRowColumnEnum.ColumnA: return DataRowColumnEnum.ColumnB;
            case DataRowColumnEnum.ColumnB: return DataRowColumnEnum.ColumnC;
            case DataRowColumnEnum.ColumnC: return DataRowColumnEnum.ColumnD;
            case DataRowColumnEnum.ColumnD: return DataRowColumnEnum.ColumnE;
            case DataRowColumnEnum.ColumnE: return DataRowColumnEnum.ColumnF;
            case DataRowColumnEnum.ColumnF: return DataRowColumnEnum.ColumnG;
            case DataRowColumnEnum.ColumnG: return DataRowColumnEnum.ColumnA;
            default: return DataRowColumnEnum.ColumnA;
        }
    }

    /// <summary>
    /// Simply returns the text that represents a column enum.
    /// </summary>
    /// <param name="current">Current enum value</param>
    /// <returns>Label of column</returns>
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

    /// <summary>
    /// Simply returns the next data row aggregator value based on current.
    /// </summary>
    /// <param name="current">Current enum value</param>
    /// <returns>Next enum value</returns>
    private DataQueryAggregatorEnum NextAggregatorEnum(DataQueryAggregatorEnum current)
    {
        switch (current)
        {
            case DataQueryAggregatorEnum.None: return DataQueryAggregatorEnum.Min;
            case DataQueryAggregatorEnum.Min: return DataQueryAggregatorEnum.Max;
            case DataQueryAggregatorEnum.Max: return DataQueryAggregatorEnum.Avg;
            case DataQueryAggregatorEnum.Avg: return DataQueryAggregatorEnum.Sum;
            case DataQueryAggregatorEnum.Sum: return DataQueryAggregatorEnum.Count;
            case DataQueryAggregatorEnum.Count: return DataQueryAggregatorEnum.None;
            default: return DataQueryAggregatorEnum.None;
        }
    }

    /// <summary>
    /// Simply returns the text that represents an aggregator enum.
    /// </summary>
    /// <param name="current">Current enum value</param>
    /// <returns>Label of aggregator</returns>
    private string AggregatorEnumText(DataQueryAggregatorEnum current)
    {
        switch (current)
        {
            case DataQueryAggregatorEnum.Min: return "MIN";
            case DataQueryAggregatorEnum.Max: return "MAX";
            case DataQueryAggregatorEnum.Avg: return "AVG";
            case DataQueryAggregatorEnum.Count: return "COUNT";
            case DataQueryAggregatorEnum.Sum: return "SUM";
            case DataQueryAggregatorEnum.None: return "No\ngrp";
            default: return "No\ngrp";
        }
    }

    /// <summary>
    /// Simply returns the next data row filter operation value based on current.
    /// </summary>
    /// <param name="current">Current enum value</param>
    /// <returns>Next enum value</returns>
    private DataRowFilterOperatorEnum NextFilterEnum(DataRowFilterOperatorEnum current)
    {
        switch (current)
        {
            case DataRowFilterOperatorEnum.OperatorEqual: return DataRowFilterOperatorEnum.OperatorNotEqual;
            case DataRowFilterOperatorEnum.OperatorNotEqual: return DataRowFilterOperatorEnum.OperatorLessThan;
            case DataRowFilterOperatorEnum.OperatorLessThan: return DataRowFilterOperatorEnum.OperatorLessThanOrEqual;
            case DataRowFilterOperatorEnum.OperatorLessThanOrEqual: return DataRowFilterOperatorEnum.OperatorGreaterThan;
            case DataRowFilterOperatorEnum.OperatorGreaterThan: return DataRowFilterOperatorEnum.OperatorGreaterThanOrEqual;
            case DataRowFilterOperatorEnum.OperatorGreaterThanOrEqual: return DataRowFilterOperatorEnum.OperatorEqual;
            default: return DataRowFilterOperatorEnum.OperatorEqual;
        }
    }

    /// <summary>
    /// Simply returns the next data row filter operation value based on current.
    /// </summary>
    /// <param name="current">Current enum value</param>
    /// <returns>Next enum value</returns>
    private DataRowFilterOperatorEnum NextCombinationFilterEnum(DataRowFilterOperatorEnum current)
    {
        switch (current)
        {
            case DataRowFilterOperatorEnum.OperatorNone: return DataRowFilterOperatorEnum.OperatorAnd;
            case DataRowFilterOperatorEnum.OperatorAnd: return DataRowFilterOperatorEnum.OperatorOr;
            case DataRowFilterOperatorEnum.OperatorOr: return DataRowFilterOperatorEnum.OperatorNone;
            default: return DataRowFilterOperatorEnum.OperatorNone;
        }
    }

    /// <summary>
    /// Simply returns the text for filter operation value based on current.
    /// </summary>
    /// <param name="current">Current enum value</param>
    /// <returns>Label of filter value</returns>
    private string FilterEnumText(DataRowFilterOperatorEnum current)
    {
        switch (current)
        {
            case DataRowFilterOperatorEnum.OperatorNone: return "-";
            case DataRowFilterOperatorEnum.OperatorEqual: return "=";
            case DataRowFilterOperatorEnum.OperatorNotEqual: return "<>";
            case DataRowFilterOperatorEnum.OperatorLessThan: return "<";
            case DataRowFilterOperatorEnum.OperatorLessThanOrEqual: return "<=";
            case DataRowFilterOperatorEnum.OperatorGreaterThan: return ">";
            case DataRowFilterOperatorEnum.OperatorGreaterThanOrEqual: return ">=";
            case DataRowFilterOperatorEnum.OperatorAnd: return "AND";
            case DataRowFilterOperatorEnum.OperatorOr: return "OR";
            default: return "-";
        }
    }

}