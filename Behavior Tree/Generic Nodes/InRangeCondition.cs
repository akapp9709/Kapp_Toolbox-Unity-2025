using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeCondition : ConditionalNode
{
    [SerializeField] private float maxRange;

    public InRangeCondition(BlackBoard blackBoard) : base(blackBoard)
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
        if(blackBoard.GetValue("DistanceToTarget", out object obj))
        {
            var val = (float)obj;
            success = val <= maxRange;
        }


        return Condition(success);
    }
}
