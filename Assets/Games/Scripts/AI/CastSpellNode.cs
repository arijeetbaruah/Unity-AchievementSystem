using System;
using System.Collections.Generic;
using UnityEngine;

public class CastSpellNode : BaseNode
{
    [Input(connectionType = ConnectionType.Override)]
    public string previousNode;
    [Output(connectionType = ConnectionType.Override)]
    public string nextNode;

    public Spell spell;

    public override void Execute(List<CharacterDetails> targets, CharacterDetails characterDetails, Action<bool> callback)
    {
        spell.Execute(targets, characterDetails, callback);
    }

    public override void Update()
    {

    }

    public override BaseNode GetNextNode()
    {
        return GetOutputPort("nextNode").Connection.node as BaseNode;
    }
}
