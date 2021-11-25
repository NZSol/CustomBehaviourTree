using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : BTCoreNode
{
    protected List<BTCoreNode> nodes = new List<BTCoreNode>();
    string curNode;
    public Sequencer(List<BTCoreNode> nodes, TreeFunc myAI, string curNode)
    {
        this.nodes = nodes;
        this.myAI = myAI;
        this.curNode = curNode;
    }

    public override NodeState Evaluate()
    {
        myAI.nodePrint(curNode);
        bool isNodeRunning = false;
        foreach(var node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.RUNNING:
                    isNodeRunning = true;
                    break;
                case NodeState.SUCCESS:

                    break;
                case NodeState.FAILURE:
                    _state = NodeState.FAILURE;
                    return _state;
            }
        }
        _state = isNodeRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        return _state;
    }
}
