using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseNode : ActionNode
{
    private Transform _target;


    public SenseNode(BlackBoard blackBoard) : base(blackBoard)
    {
    }

    protected override void OnStart()
    {
        if(blackBoard.GetValue<Transform>("Target", out _target))
            Debug.Log("Target Found");
    }

    protected override void OnStop()
    {
        //I need to figure out what this bug is
    }

    protected override State OnUpdate()
    {
        Debug.Log("Sensing...");
        return State.RUNNING;
    }
}
