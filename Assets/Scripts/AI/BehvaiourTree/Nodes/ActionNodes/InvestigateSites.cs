using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InvestigateSites : BTCoreNode
{
    Queue<Vector3> positions = new Queue<Vector3>();
    NavMeshAgent agent;
    float waitTime;

    public InvestigateSites(Queue<Vector3> positions, NavMeshAgent agent, float waitTime, TreeFunc myAI)
    {
        this.positions = positions;
        this.agent = agent;
        this.waitTime = waitTime;
        this.myAI = myAI;
    }

    public override NodeState Evaluate()
    {
        throw new System.NotImplementedException();
    }

}
