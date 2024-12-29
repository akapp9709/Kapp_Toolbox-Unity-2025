using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
        BehaviorTree tree = Selection.activeObject as BehaviorTree;
        if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
        {
            treeView.PopulateView(tree);
        }
    }

    private void OnNodeSelectionChanged(NodeView node)
    {
        inspectorView.UpdateSelection(node);
    }
}
