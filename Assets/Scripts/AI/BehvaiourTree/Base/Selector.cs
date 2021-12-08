using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : BTCoreNode
{
    protected List<BTCoreNode> nodes = new List<BTCoreNode>();
    string curNode;
    public Selector(List<BTCoreNode> nodes, TreeFunc myAI, string curNode)
    {
        this.myAI = myAI;
        this.nodes = nodes;
        this.curNode = curNode;
    }

    public override string ToString()
    {
        return ($"Selector: {curNode}");
    }

    public override NodeState Evaluate()
    {
        myAI.nodePrint(this);
        foreach (var node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.RUNNING:
                    _state = NodeState.RUNNING;
                    return _state;

                case NodeState.SUCCESS:
                    _state = NodeState.SUCCESS;
                    return _state;

                case NodeState.FAILURE:
                    break;
            }
        }
        _state = NodeState.FAILURE;
        return _state;
    }

    public override void CleanUp()
    {
        foreach (BTCoreNode node in nodes)
        {
            node.CleanUp();
        }
    }
}
