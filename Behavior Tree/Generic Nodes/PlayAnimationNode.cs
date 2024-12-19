using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationNode : ActionNode
{
    [SerializeField] private string animationStateName;
    [Range(0, 1)][SerializeField] private float fadeTime;

    bool isPlaying = false;
    Animator animator;

    public PlayAnimationNode(BlackBoard blackBoard) : base(blackBoard)
    {
        // Need tyo find this fucking bug. Agian. Again
    }

    protected override void OnStart()
    {
        Debug.Log("Starting Animation Node");

        if (blackBoard.GetValue<Animator>("Animator", out animator))
        {
            Debug.Log("Animator Found");
            isPlaying = true;
            animator.CrossFade(animationStateName, fadeTime);
        }
        else
            Debug.Log("Animator Lost");
    }

    protected override void OnStop()
    {
        started = false;
        isPlaying = false;
    }

    protected override State OnUpdate()
    {
        Debug.Log("Animation Node Running   " + animationStateName);
        if (!isPlaying)
            return State.FAILURE;

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(animationStateName))
        {
            if (stateInfo.normalizedTime >= 1.0f)
            {
                isPlaying = false;
                Debug.Log("Animation Complete");
                return State.SUCCESS;
            }
            else
            {
                isPlaying = true;
                return State.RUNNING;
            }
        }

        Debug.Log("Exited animation early");
        return State.FAILURE;
    }
}

