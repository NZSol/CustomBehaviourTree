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

    public override void onNodeEnter()
    {
        base.onNodeEnter();
        Debug.Log("HIt");
        timer = 0;
    }


    public override NodeState Evaluate()
    {
        if (myAI.curNode != this)
        {
            onNodeEnter();
        }

        canCount = myAI.canCount;

        myAI.nodePrint(this);
        switch (timer < waitTime)
        {
            case true:
                if (canCount)
                {
                    timer += Time.deltaTime;
                    myAI.SetColor(Color.black);
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
                    myAI.playerFound = false;
                    myAI.canCount = false;
                }
                timer -= waitTime;
                _state = NodeState.SUCCESS;
                myAI.canCount = false;
                break;
        }
        return _state;
    }
}
