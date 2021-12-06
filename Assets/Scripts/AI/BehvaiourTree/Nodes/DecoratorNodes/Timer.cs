using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : BTCoreNode
{
    protected float waitTime;
    float timer;
    bool canCount;
    string nodeName;
    bool canRun;
    public Timer(float waitTime, TreeFunc myAI, string nodeName)
    {
        this.waitTime = waitTime;
        this.myAI = myAI;
        this.nodeName = nodeName;
    }

    public override string ToString()
    {
        return ($"{nodeName} {timer}");
    }

    public override NodeState Evaluate()
    {
        canCount = myAI.canCount;
        canRun = myAI.canRun;

        myAI.nodePrint(this);

        switch(timer < waitTime)
        {
            case true:
                if (canCount && canRun)
                {
                    timer += Time.deltaTime;
                    timer = waitTime;
                    _state = NodeState.RUNNING;
                }
                else
                {
                    timer = 0;
                }
                break;
            case false:
                if (myAI.playerFound)
                {
                    myAI.canCount = false;
                }
                timer -= waitTime;
                _state = NodeState.SUCCESS;
                break;
        }
        return _state;
    }
}
