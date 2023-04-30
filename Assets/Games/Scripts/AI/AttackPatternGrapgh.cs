using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class AttackPatternGrapgh : NodeGraph {

	public BaseNode currentNode;

	public void Initialize()
	{
		currentNode = nodes.Find(n => n is RootNode) as BaseNode;
    }

	public void Update()
	{
		currentNode?.Update();
	}

	public void GotoNext()
	{
		var node = currentNode.GetNextNode();
		if (node == null )
		{
			node = nodes.Find(n => n is RootNode) as BaseNode;
		}

		currentNode = node;
	}
}