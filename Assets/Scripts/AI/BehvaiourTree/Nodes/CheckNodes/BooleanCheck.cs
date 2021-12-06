using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooleanCheck : BTCoreNode
{
    bool player, hiding;

    public BooleanCheck(bool value, TreeFunc myAI)
    {
        this.myAI = myAI;
    }

    public override NodeState Evaluate()
    {
        player = myAI.playerFound;
        hiding = myAI.hidingFound;
        myAI.nodePrint(this);
        switch (player || hiding)
        {
            case true:
                _state = NodeState.SUCCESS;
                break;
            case false:
                _state = NodeState.FAILURE;
                break;
        }
        return _state;
    }
}
