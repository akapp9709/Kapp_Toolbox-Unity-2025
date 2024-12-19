using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelNode : CompositeNode
{
    public ParallelNode(BlackBoard blackBoard) : base(blackBoard)
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
        bool allSuccess = true;

        foreach (var child in children)
        {
            if(child.Evaluate(blackBoard) == State.FAILURE)
                allSuccess = false;
        }

        return allSuccess ? State.RUNNING : State.FAILURE;
    }
}
