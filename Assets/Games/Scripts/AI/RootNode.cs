using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static XNode.Node;

public class RootNode : BaseNode
{
    [Output(connectionType = ConnectionType.Override)]
    public string nextNode;

    public override void Execute(List<CharacterDetails> targets, CharacterDetails characterDetails, Action<bool> callback)
    {
        
    }

    public override void Update()
    {
        
    }

    public override BaseNode GetNextNode()
    {
        return GetOutputPort("nextNode").Connection.node as BaseNode;
    }
}
