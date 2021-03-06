using System.Collections;
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
    }

    public abstract NodeState Evaluate();

    public virtual void CleanUp()
    {
    }
}
