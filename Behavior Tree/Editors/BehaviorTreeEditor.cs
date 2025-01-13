using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;

namespace BehaviorTree
{
    public class BehaviorTreeEditor : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        BehaviorTreeView treeView;
        InspectorView inspectorView;

        [MenuItem("BehaviorTreeEditor/Editor ...")]
        public static void OpenWindow()
        {
            BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
            wnd.titleContent = new GUIContent("BehaviorTreeEditor");
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int insstanceId, int line)
        {
            if (Selection.activeObject is BehaviorTreeClass)
            {
                OpenWindow();
                return true;
            }
            return false;
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
            var vsGuids = AssetDatabase.FindAssets("BehaviorTreeEditor t:VisualTreeAsset");
            string vtaPath = AssetDatabase.GUIDToAssetPath(vsGuids.FirstOrDefault());
            Debug.Log(vtaPath);

            var ssGuids = AssetDatabase.FindAssets("BehaviorTreeEditor t:StyleSheet");
            string ssPath = AssetDatabase.GUIDToAssetPath(ssGuids.FirstOrDefault());
            Debug.Log(ssPath);

            if (root == null)
            {
                Debug.Log("this fucking thing is missing");
            }

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(vtaPath);
            visualTree.CloneTree(root);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ssPath);
            root.styleSheets.Add(styleSheet);

            treeView = root.Q<BehaviorTreeView>();
            inspectorView = root.Q<InspectorView>();

            treeView.OnNodeSelected = OnNodeSelectionChanged;


            OnSelectionChange();
        }



        private void OnSelectionChange()
        {
            BehaviorTreeClass tree = Selection.activeObject as BehaviorTreeClass;

            if (!tree)
            {
                if (Selection.activeGameObject)
                {
                    Brain btBrain = Selection.activeGameObject.GetComponent<Brain>();
                    if (btBrain)
                    {
                        tree = btBrain.tree;
                    }
                }

                if (Application.isPlaying)
                {
                    if (tree)
                    {
                        treeView.PopulateView(tree);
                    }
                }

            }
            else
            {
                if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
                    treeView.PopulateView(tree);
            }
        }

        private void OnNodeSelectionChanged(NodeView node)
        {
            inspectorView.UpdateSelection(node);
        }
        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnPlayStateChanged;
            EditorApplication.playModeStateChanged += OnPlayStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayStateChanged;
        }

        private void OnPlayStateChanged(PlayModeStateChange change)
        {
            switch (change)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                default:
                    break;
            }
        }
    }
}
