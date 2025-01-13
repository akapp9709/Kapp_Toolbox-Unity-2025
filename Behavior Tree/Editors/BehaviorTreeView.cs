using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;
using TreeEditor;
using System.Linq;
using BehaviorTree;


public class BehaviorTreeView : GraphView
{
    public Action<NodeView> OnNodeSelected;
    public new class UxmlFactory : UxmlFactory<BehaviorTreeView, GraphView.UxmlTraits> { }
    BehaviorTreeClass tree;

    public BehaviorTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Behavior Tree/Editors/BehaviorTreeEditor.uss");
        styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnUndoRedo()
    {
        PopulateView(tree);

        AssetDatabase.SaveAssets();
    }

    internal void PopulateView(BehaviorTreeClass tree)
    {
        this.tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        if (tree.root == null)
        {
            tree.root = tree.CreateNode(typeof(RootNode)) as RootNode;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }

        tree.nodes.ForEach(n => CreateNodeView(n));

        tree.nodes.ForEach(n =>
        {
            var children = tree.GetChildren(n);
            children.ForEach(c =>
            {
                var parent = FindNodeView(n);
                var child = FindNodeView(c);

                var edge = parent.output.ConnectTo(child.input);
                AddElement(edge);
            });
        });
    }

    private NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    tree.DeleteNode(nodeView.node);
                }

                Edge edge = elem as Edge;


                if (edge != null)
                {
                    if (edge.output == null || edge.input == null)
                    {
                        Debug.Log("Orphaned Edge");
                    }

                    var parent = edge.output.node as NodeView;
                    var child = edge.input.node as NodeView;
                    tree.RemoveChild(parent.node, child.node);
                }
            });
        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                var parent = edge.output.node as NodeView;
                var child = edge.input.node as NodeView;
                tree.AddChild(parent.node, child.node);
            });
        }

        if (graphViewChange.movedElements != null)
        {
            nodes.ForEach(n =>
            {
                var view = n as NodeView;
                view.SortChildren();
            });
        }
        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
        foreach (var type in types)
        {
            evt.menu.AppendAction($"Actions/{type.Name}", (a) => CreateNode(type));
        }

        types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
        foreach (var type in types)
        {
            evt.menu.AppendAction($"Composites/{type.Name}", (a) => CreateNode(type));
        }

        types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
        foreach (var type in types)
        {
            if (typeof(ConditionalNode).IsAssignableFrom(type))
                evt.menu.AppendAction($"Decorators/Conditional/{type.Name}", (a) => CreateNode(type));
            else
                evt.menu.AppendAction($"Decorators/{type.Name}", (a) => CreateNode(type));
        }
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }

    void CreateNode(System.Type type)
    {
        var node = tree.CreateNode(type);
        CreateNodeView(node);
    }

    void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }

    public void SortTreeNodes()
    {
        tree.SortChildren();
    }
}
