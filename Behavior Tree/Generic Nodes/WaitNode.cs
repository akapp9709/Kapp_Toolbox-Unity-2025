using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : ActionNode
{
    public float duration = 1;
    float startTime = 0;

    public WaitNode(BlackBoard blackBoard) : base(blackBoard)
    {
        
    }

    protected override void OnStart()
    {
        startTime = Time.time;
        Debug.Log("Restarting Delay");
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(Time.time - startTime > duration){
            return State.SUCCESS;
        }
        return State.RUNNING;
    }
}
