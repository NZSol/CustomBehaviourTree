using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableBool : BTCoreNode
{
    string input;

    public DisableBool(string input, TreeFunc myAI)
    {
        this.myAI = myAI;
        this.input = input;
    }


    public override NodeState Evaluate()
    {
        Debug.Log(input);
        switch (input)
        {
            case "playerFound":
                myAI.playerFound = false;
                Debug.Log("Hit Player");
                break;
            case "hidingFound":
                Debug.Log("Hit Hiding");
                myAI.hidingFound = false;
                break;
        }
        myAI.nodePrint(this);
        return NodeState.SUCCESS;
    }

}
