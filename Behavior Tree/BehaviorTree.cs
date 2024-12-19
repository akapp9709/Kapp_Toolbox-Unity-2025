using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CreateAssetMenu()]
public class BehaviorTree : ScriptableObject
{
    public Node root;
    public Node.State treeState = Node.State.RUNNING;
    public List<Node> nodes = new List<Node>();
    private BlackBoard blackBoard = new BlackBoard();

    public Node.State Update()
    {
        if (root.state == Node.State.RUNNING)
            treeState = root.Evaluate(blackBoard);
        return treeState;
    }

    public Node CreateNode(System.Type type)
    {
        Node node = ScriptableObject.CreateInstance(type) as Node;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode(Node node)
    {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(Node parent, Node child)
    {
        RootNode root = parent as RootNode;
        if (root)
            root.child = child;

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
            decorator.child = child;

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            if (composite == null)
                Debug.Log("Not Seeing Composite");
            composite.children.Add(child);
        }
    }

    public void SortChildren()
    {
        root.SortChildren();
    }

    public void RemoveChild(Node parent, Node child)
    {
        RootNode root = parent as RootNode;
        if (root)
            root.child = null;

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
            decorator.child = null;

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            composite.children.Remove(child);

        }

    }

    public List<Node> GetChildren(Node parent)
    {
        var list = new List<Node>();

        RootNode root = parent as RootNode;
        if (root && root.child != null)
            list.Add(root.child);

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator && decorator.child != null)
            list.Add(decorator.child);

        CompositeNode composite = parent as CompositeNode;
        if (composite)
            return composite.children;

        return list;
    }

    public BehaviorTree Clone()
    {
        BehaviorTree tree = Instantiate(this);
        tree.root = tree.root.Clone();

        var keys = blackBoard.GetAllKeys();
        string keyCheck = "(";
        foreach (var key in keys)
        {
            keyCheck += key + ", ";
        }
        Debug.Log(keyCheck + ")      On Clone");

        return tree;
    }

    public void WriteData(string id, object value)
    {
        blackBoard.SetValue(id, value);
    }
}
