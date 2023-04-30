using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

[CustomNodeGraphEditor(typeof(AttackPatternGrapgh))]
public class AttackPatternGrapghEditor : NodeGraphEditor
{
    public override void OnCreate()
    {
        base.OnCreate();
        CreateRootNode();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        CreateRootNode();
    }

    private void CreateRootNode()
    {
        AttackPatternGrapgh attackPattern = (AttackPatternGrapgh)target;
        int nodeCount = attackPattern.nodes.Where(n => n is RootNode).Count();
        if (nodeCount == 0)
        {
            RootNode node = attackPattern.AddNode<RootNode>();
            node.name = "Root Node";
            AssetDatabase.AddObjectToAsset(node, attackPattern);
        }
    }
}
