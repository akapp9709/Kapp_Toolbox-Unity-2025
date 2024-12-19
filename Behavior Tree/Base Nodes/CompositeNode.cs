using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public abstract class CompositeNode : Node
{
    public List<Node> children = new List<Node>();

    public CompositeNode(BlackBoard blackBoard) : base(blackBoard)
    {

    }

    public override Node Clone()
    {
        CompositeNode node = Instantiate(this);
        node.children = children.ConvertAll(c => c.Clone());
        return node;
    }

    public override void SortChildren()
    {
        children.Sort((a, b) => a.position.y.CompareTo(b.position.y));
        foreach (var child in children)
        {
            child.SortChildren();
        }
    }
}
