using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecoratorNode : Node
{
    public Node child;

    public DecoratorNode(BlackBoard blackBoard) : base(blackBoard)
    {
        this.blackBoard = blackBoard;
    }

    public override Node Clone()
    {
        DecoratorNode node = Instantiate(this);
        node.child = child.Clone();
        return node;
    }

    public override void SortChildren()
    {
        child.SortChildren();
    }
}
