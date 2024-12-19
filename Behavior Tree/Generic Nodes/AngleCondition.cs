using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AngleCondition : ConditionalNode
{
    [SerializeField] float maxAngle, minAngle;
    public AngleCondition(BlackBoard blackBoard) : base(blackBoard)
    {

    }

    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(blackBoard.GetValue("TargetAngle", out object obj))
        {
            var val = (float)obj;
            success = val <= maxAngle && val >= minAngle;
        }


        return Condition(success);
    }
}
