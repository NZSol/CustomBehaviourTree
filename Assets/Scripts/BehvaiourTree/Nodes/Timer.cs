using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : BTCoreNode
{
    protected BTCoreNode node;
    protected float waitTime;
    public Timer(BTCoreNode node, float waitTime)
    {
        this.node = node;
        this.waitTime = waitTime;
    }

    public override NodeState Evaluate()
    {
        myAI.nodePrint(this);
        float timer = 0;
        while (timer < waitTime)
        {
            timer += Time.deltaTime;
            Debug.Log("Timer: " + timer);
        }
        _state = NodeState.SUCCESS;
        return _state;
    }
}
