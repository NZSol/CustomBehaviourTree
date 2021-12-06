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
        switch (input)
        {
            case "playerFound":
                myAI.playerFound = false;
                break;
            case "hidingFound":
                myAI.hidingFound = false;
                break;
        }
        myAI.nodePrint(this);
        return NodeState.SUCCESS;
    }

}
