using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;


namespace BehaviorTree
{
    [CreateAssetMenu()]
    public class BehaviorTreeClass : ScriptableObject
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

            Undo.RecordObject(this, "Behavior Tree (CreateNode)");
            nodes.Add(node);

            if (!Application.isPlaying)
                AssetDatabase.AddObjectToAsset(node, this);

            Undo.RegisterCreatedObjectUndo(node, "BEhavior Tree (CreateNode)");

            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(Node node)
        {
            Undo.RegisterCreatedObjectUndo(node, "BEhavior Tree (DeleteNode)");
            nodes.Remove(node);
            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parent, Node child)
        {
            RootNode root = parent as RootNode;
            if (root)
            {
                Undo.RecordObject(root, "Behavior Tree (AddChild)");
                root.child = child;

                EditorUtility.SetDirty(root);
            }

            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Behavior Tree (AddChild)");
                decorator.child = child;
                EditorUtility.SetDirty(decorator);
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                Undo.RecordObject(composite, "Behavior Tree (AddChild)");
                composite.children.Add(child);
                EditorUtility.SetDirty(composite);
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
            {
                Undo.RecordObject(root, "Behavior Tree (RemoveChild)");
                root.child = null;
                EditorUtility.SetDirty(root);
            }

            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Behavior Tree (RemoveChild)");
                decorator.child = null;
                EditorUtility.SetDirty(decorator);
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                Undo.RecordObject(composite, "Behavior Tree (RemoveChild)");
                composite.children.Remove(child);
                EditorUtility.SetDirty(composite);
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

        public BehaviorTreeClass Clone()
        {
            BehaviorTreeClass tree = Instantiate(this);
            tree.root = tree.root.Clone();
            tree.nodes = new List<Node>();
            Traverse(tree.root, n =>
            {
                tree.nodes.Add(n);
            });

            var keys = blackBoard.GetAllKeys();
            string keyCheck = "(";
            foreach (var key in keys)
            {
                keyCheck += key + ", ";
            }
            Debug.Log(keyCheck + ")      On Clone");

            return tree;
        }

        public void Traverse(Node node, System.Action<Node> visiter)
        {
            if (node)
            {
                visiter.Invoke(node);
                var children = GetChildren(node);
                children.ForEach((n) => Traverse(n, visiter));
            }
        }

        public void WriteData(string id, object value)
        {
            blackBoard.SetValue(id, value);
        }
    }
}
