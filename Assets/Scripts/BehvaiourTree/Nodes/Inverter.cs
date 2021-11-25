using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : BTCoreNode
{
    protected BTCoreNode node;
    public Inverter(BTCoreNode node, TreeFunc myAI)
    {
        this.node = node;
        this.myAI = myAI;
    }

    public override NodeState Evaluate()
    {
        switch (node.Evaluate())
        {
            case NodeState.RUNNING:
                _state = NodeState.RUNNING;
                break;
            case NodeState.SUCCESS:
                _state = NodeState.FAILURE;
                break;
            case NodeState.FAILURE:
                _state = NodeState.SUCCESS;
                break;
        }
        return _state;
    }
}
