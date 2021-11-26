using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : BTCoreNode
{
    Transform Player;
    NavMeshAgent agent;

    public Chaser(Transform Player, NavMeshAgent agent, TreeFunc myAI)
    {
        this.myAI = myAI;
        this.Player = Player;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        myAI.nodePrint(this);
        float dist = Vector3.Distance(Player.position, agent.transform.position);
        if (dist > 0.5f)
        {
            agent.isStopped = false;
            myAI.SetColor(Color.blue);
            agent.SetDestination(Player.position);
            return NodeState.RUNNING;
        }
        else
        {
            return NodeState.SUCCESS;
        }
    }
}
