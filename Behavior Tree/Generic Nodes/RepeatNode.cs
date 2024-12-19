using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : DecoratorNode
{
    public RepeatNode(BlackBoard blackBoard) : base(blackBoard)
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
        child.Evaluate(blackBoard);
        return State.RUNNING;
    }
}
