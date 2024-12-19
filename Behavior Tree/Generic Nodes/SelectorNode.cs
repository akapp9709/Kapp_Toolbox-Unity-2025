using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode
{
    private int current = 0;
    public SelectorNode(BlackBoard blackBoard) : base(blackBoard)
    {
    }

    protected override void OnStart()
    {
        current = 0;
        Debug.Log($"Checking {children.Count} branches");
    }

    protected override void OnStop()
    {
        started = false;
    }

    protected override State OnUpdate()
    {
        if (current >= children.Count)
        {
            return State.FAILURE;
        }

        var child = children[current];
        switch (child.Evaluate(blackBoard))
        {
            case State.RUNNING:
                return State.RUNNING;
            case State.FAILURE:
                current++;
                break;
            case State.SUCCESS:
                return State.SUCCESS;
        }

        return current < children.Count ? State.RUNNING : State.FAILURE;
    }
}
