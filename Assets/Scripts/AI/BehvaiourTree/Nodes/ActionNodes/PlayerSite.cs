using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerSite : BTCoreNode
{
    NavMeshAgent agent;
    Transform player;
    Vector3 target;
    Vector3 position;
    bool targetSet = false;

    public PlayerSite(NavMeshAgent agent, Transform player, Vector3 target, TreeFunc myAI)
    {
        this.agent = agent;
        this.player = player;
        this.myAI = myAI;
    }

    public override NodeState Evaluate()
    {
        target = myAI.target;
        myAI.nodePrint(this);
#if UNITY_EDITOR
        GameObject obj = GameObject.FindWithTag("Respawn");
        obj.transform.position = target;
#endif
        float dist = Vector3.Distance(target, agent.transform.position);
        if (dist > 0.5f)
        {
            agent.isStopped = false;
            agent.SetDestination(target);
            myAI.SetColor(Color.cyan);
            _state = NodeState.RUNNING;
            Debug.Log(dist);
        }
        else
        {
            agent.isStopped = true;
            myAI.canCount = true;
            targetSet = false;
            _state = NodeState.SUCCESS;
            Debug.Log("Test More");
        }
        return _state;
    }
}
