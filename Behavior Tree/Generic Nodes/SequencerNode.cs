using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerNode : CompositeNode
{
    private int current;

    public SequencerNode(BlackBoard blackBoard) : base(blackBoard)
    {
        
    }

    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(current == children.Count)
            return State.SUCCESS;

        var child = children[current];
        switch (child.Evaluate(blackBoard))
        {
            case State.RUNNING:
                return State.RUNNING;
            case State.FAILURE:
                return State.FAILURE;
            case State.SUCCESS:
                current++;
                break;
        }

        return current == children.Count ? State.SUCCESS : State.RUNNING;
    }
}
