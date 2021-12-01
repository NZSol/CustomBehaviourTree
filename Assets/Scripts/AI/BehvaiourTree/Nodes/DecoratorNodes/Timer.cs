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

    //public override void onNodeEnter()
    //{
    //    base.onNodeEnter();
    //    timer = 0;
    //}


    public override NodeState Evaluate()
    {
        canCount = myAI.canCount;

        myAI.nodePrint(this);
        switch (timer < waitTime)
        {
            case true:
                if (canCount)
                {
                    timer += Time.deltaTime;
                    //myAI.SetColor(Color.black);
                    _state = NodeState.RUNNING;
                    return _state;
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
                _state = NodeState.SUCCESS;
                myAI.canCount = false;
                if(myAI.targetHideable != null)
                {
                    myAI.targetHideable = null;
                }
                break;
        }
        Debug.Log($"Hit Timer. TimerWait = {waitTime}. My current timer = {timer}");
        return _state;
    }
}
