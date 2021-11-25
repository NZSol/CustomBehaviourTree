using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetect : BTCoreNode
{
    bool invert;
    public PlayerDetect(TreeFunc myAI, bool invert)
    {
        this.myAI = myAI;
        this.invert = invert;
    }

    public override string ToString()
    {
        return "PlayerDetect" + " Inverted == " + invert;
    }

    public override NodeState Evaluate()
    {
        myAI.nodePrint(this);
        myAI.SetColor(Color.green);
        if (!invert && myAI.CanSeePlayer())
        {
            myAI.playerFound = true;
        }
        return myAI.CanSeePlayer() ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
