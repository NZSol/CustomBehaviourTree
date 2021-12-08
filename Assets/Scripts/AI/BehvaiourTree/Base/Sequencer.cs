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

    public override string ToString()
    {
        return ($"Sequencer: {curNode}");
    }

    public override NodeState Evaluate()
    {
        myAI.nodePrint(this);
        foreach(var node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.RUNNING:
                    _state = NodeState.RUNNING;
                    return _state;

                case NodeState.SUCCESS:
                    break;

                case NodeState.FAILURE:
                    _state = NodeState.FAILURE;
                    return _state;
            }
        }
        _state = NodeState.SUCCESS;
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
