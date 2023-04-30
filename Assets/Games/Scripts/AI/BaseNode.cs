using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public abstract class BaseNode : Node
{
    public abstract BaseNode GetNextNode();

    public abstract void Execute(List<CharacterDetails> targets, CharacterDetails characterDetails, Action<bool> callback);
    public abstract void Update();
}
