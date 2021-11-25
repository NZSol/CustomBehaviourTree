using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : BTCoreNode
{
    string message;
    public Printer(string message)
    {
        this.message = message;
    }

    public override NodeState Evaluate()
    {
        return NodeState.SUCCESS;
    }
}
