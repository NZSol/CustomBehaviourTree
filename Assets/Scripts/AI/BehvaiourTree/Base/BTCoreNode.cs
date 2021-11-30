﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BTCoreNode
{
    public enum NodeState { RUNNING, FAILURE, SUCCESS };

    protected NodeState _state;
    protected TreeFunc myAI;

    public NodeState state { get { return _state; } }
    public virtual void onNodeEnter()
    {
        myAI.curNode = this;
        Debug.Log("Test");
    }

    public abstract NodeState Evaluate();

}
