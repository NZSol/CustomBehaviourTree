using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooleanCheck : BTCoreNode
{
    bool value;
    public BooleanCheck(bool value, TreeFunc myAI)
    {
        this.value = value;
        this.myAI = myAI;
    }

    public override NodeState Evaluate()
    {
        value = myAI.playerFound;
        myAI.nodePrint(this);
        switch (value)
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
