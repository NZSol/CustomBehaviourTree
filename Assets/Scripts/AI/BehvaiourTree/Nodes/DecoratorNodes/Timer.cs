using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : BTCoreNode
{
    protected float waitTime;
    float timer;
    bool canCount;
    public Timer(float waitTime, TreeFunc myAI)
    {
        this.waitTime = waitTime;
        this.myAI = myAI;
    }


    public override NodeState Evaluate()
    {
        canCount = myAI.canCount;

        myAI.nodePrint(this);

        switch(timer < waitTime)
        {
            case true:
                if (canCount)
                {
                    timer += Time.deltaTime;
                    _state = NodeState.RUNNING;
                    Debug.Log(timer);
                    Debug.Break();
                }
                else
                {
                    timer = 0;
                }
                break;
            case false:
                if (myAI.playerFound)
                {
                    myAI.playerFound = false;
                    myAI.canCount = false;
                }
                timer -= waitTime;
                if (myAI.targetHideable != null)
                {
                    myAI.targetHideable = null;
                }
                _state = NodeState.SUCCESS;
                break;
        }

        return _state;
    }
}
