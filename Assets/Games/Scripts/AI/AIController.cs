using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField]
    private AttackPatternGrapgh attackPattern;

    private void Start()
    {
        attackPattern.Initialize();
        attackPattern.GotoNext();
    }

    private void Update()
    {
        attackPattern?.Update();
    }

    public void Execute(List<CharacterDetails> targets, CharacterDetails characterDetails, Action<bool> callback)
    {
        attackPattern.currentNode.Execute(targets, characterDetails, callback);
        attackPattern.GotoNext();
    }
}
