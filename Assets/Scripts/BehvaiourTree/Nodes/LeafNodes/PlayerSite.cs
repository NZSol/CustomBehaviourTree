using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerSite : BTCoreNode
{
    NavMeshAgent agent;
    Transform player;
    Vector3 position;
    bool targetSet = false;

    public PlayerSite(NavMeshAgent agent, Transform player, TreeFunc myAI)
    {
        this.agent = agent;
        this.player = player;
        this.myAI = myAI;
    }

    public override NodeState Evaluate()
    {
        myAI.nodePrint(this);
        if (!targetSet)
        {
            position = player.position;
            targetSet = true;
            Debug.Log(position);
        }
        float dist = Vector3.Distance(position, agent.transform.position);
        if (dist > 0.5f)
        {
            agent.isStopped = false;
            agent.SetDestination(position);
            myAI.SetColor(Color.cyan);
            _state = NodeState.RUNNING;
            Debug.Log("SiteRunning");
        }
        else
        {
            agent.isStopped = true;
            _state = NodeState.SUCCESS;
            targetSet = false;
            myAI.playerFound = false;
            Debug.Log("SiteComplete");
        }
        return _state;
    }
}
