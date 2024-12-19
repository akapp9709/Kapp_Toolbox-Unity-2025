using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFloat : ConditionalNode
{
    [SerializeField] private string dataID;

    public enum ConditionType
    {
        EqualTo,
        NotEqualTo,
        GreaterThan,
        GreaterThanEqualTo,
        LessThan,
        LessThanEqualTo
    }
    public ConditionType condition;
    public float value;

    public CheckFloat(BlackBoard blackBoard) : base(blackBoard)
    {
        //Fuck this bug
    }

    protected override State OnUpdate()
    {
        float data = 0;
        if(blackBoard.GetValue(dataID, out object val))
        {
            data = (float)val;
        }
        switch (condition)
        {
            case ConditionType.EqualTo:
                success = data == value;
                break;
            case ConditionType.NotEqualTo:
                success = data != value;
                break;
            case ConditionType.GreaterThan:
                success = data > value;
                break;
            case ConditionType.GreaterThanEqualTo:
                success = data >= value;
                break;
            case ConditionType.LessThan:
                success = data < value;
                break;
            case ConditionType.LessThanEqualTo:
                success = data <= value;
                break;
        }

        return Condition(success);
    }
}
