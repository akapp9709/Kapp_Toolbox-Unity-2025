using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownNode : DecoratorNode
{
    [SerializeField] float duration = 1;
    float startTime = -1;
    public CoolDownNode(BlackBoard blackBoard) : base(blackBoard)
    {
        startTime = -duration;
    }

    protected override void OnStart()
    {   
        // startTime = Time.time; AHHHHHHHHHHH
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(Time.time - startTime < duration)
            return State.FAILURE;

        startTime = Time.time;
        return child.Evaluate(blackBoard);
    }
}
