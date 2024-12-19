using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogNode : ActionNode
{
    public string message;

    public DebugLogNode(BlackBoard blackBoard) : base(blackBoard)
    {
        
    }

    protected override void OnStart()
    {
        Debug.Log($"Start Message: {message}");
    }

    protected override void OnStop()
    {
        // Debug.Log($"Stop Message: {message}");
    }

    protected override State OnUpdate()
    {
        // Debug.Log($"Update Message: {message}");
        return State.SUCCESS;
    }
}
