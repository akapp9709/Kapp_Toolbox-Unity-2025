using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : Node
{
    public Node child;

    public RootNode(BlackBoard blackBoard) : base(blackBoard)
    {

    }

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        return child.Evaluate(blackBoard);
    }

    public override Node Clone()
    {
        RootNode node = Instantiate(this);

        node.child = child.Clone();
        return node;
    }

    public override void SortChildren()
    {
        child?.SortChildren();
    }
}
