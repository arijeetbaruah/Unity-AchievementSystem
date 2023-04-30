using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class AttackNode : BaseNode
{
    [Input(connectionType = ConnectionType.Override)]
    public string previousNode;
    [Output(connectionType = ConnectionType.Override)]
    public string nextNode;

    public AttackCommand normalAttack;

    public override void Execute(List<CharacterDetails> targets, CharacterDetails characterDetails, Action<bool> callback)
    {
        normalAttack.Execute(targets[0], characterDetails, callback);
    }

    public override void Update()
    {
        normalAttack.Update();
    }

    public override BaseNode GetNextNode()
    {
        return GetOutputPort("nextNode").Connection.node as BaseNode;
    }
}