using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalNode : DecoratorNode
{
    protected bool success = false;
    public ConditionalNode(BlackBoard blackBoard) : base(blackBoard)
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
        return Condition(success);
    }

    protected State Condition(bool passed)
    {
        if(passed)
            return child.Evaluate(blackBoard);

        return State.FAILURE;
    }
}
