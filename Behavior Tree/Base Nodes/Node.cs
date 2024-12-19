using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Node : ScriptableObject, IDragAndDropEvent
{
    public enum State
    {
        RUNNING,
        FAILURE,
        SUCCESS
    }

    protected BlackBoard blackBoard;
    [HideInInspector] public State state = State.RUNNING;
    [HideInInspector] public bool started = false;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;

    public Node(BlackBoard blackBoard)
    {
        // this.blackBoard = blackBoard;
    }

    public State Evaluate(BlackBoard bb)
    {
        this.blackBoard = bb;
        if (!started)
        {
            OnStart();
            started = true;
        }

        state = OnUpdate();

        if (state == State.FAILURE || state == State.SUCCESS)
        {
            OnStop();
            started = false;
        }

        return state;
    }

    public virtual Node Clone()
    {
        var node = Instantiate(this);
        return node;
    }

    public abstract void SortChildren();

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}
