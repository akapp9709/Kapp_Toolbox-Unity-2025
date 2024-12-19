using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionNode : Node
{
    public ActionNode(BlackBoard blackBoard) : base(blackBoard)
    {
        this.blackBoard = blackBoard;
    }

    public override void SortChildren()
    {
        return;
    }
}
