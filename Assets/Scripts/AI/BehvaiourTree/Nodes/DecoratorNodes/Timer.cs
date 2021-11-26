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
        switch (timer < waitTime)
        {
            case true:
                Debug.Log("Case");
                if (canCount)
                {
                    Debug.Log(canCount);
                    timer += Time.deltaTime;
                    myAI.SetColor(Color.black);
                    _state = NodeState.RUNNING;
                }
                break;
            case false:
                if (myAI.playerFound)
                {
                    myAI.playerFound = false;
                }
                timer -= waitTime;
                _state = NodeState.SUCCESS;
                break;
        }
        Debug.Log(timer);
        Debug.Log(_state);
        return _state;
    }
}
