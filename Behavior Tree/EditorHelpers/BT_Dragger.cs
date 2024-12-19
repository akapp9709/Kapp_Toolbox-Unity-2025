using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

class BT_NodeDragger : Dragger
{
    public BT_NodeDragger(NodeView node)
    {
        target = node;

        RegisterCallbacksOnTarget();

    }

    protected new void OnMouseDown(UnityEngine.UIElements.MouseDownEvent e)
    {
        base.OnMouseDown(e);
        Debug.Log("Click");
    }
}