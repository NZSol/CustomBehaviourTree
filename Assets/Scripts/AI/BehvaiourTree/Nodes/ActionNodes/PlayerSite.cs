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
    bool targetPlayer = false;
    float range;

    string nodeName;

    public PlayerSite(NavMeshAgent agent, Transform player, TreeFunc myAI, bool targetPlayer, string nodeName)
    {
        this.agent = agent;
        this.player = player;
        this.myAI = myAI;
        this.nodeName = nodeName;
        this.targetPlayer = targetPlayer;
    }

    public override string ToString()
    {
        return ($"Move To Site: {nodeName}");
    }

    public override NodeState Evaluate()
    {
        switch (targetPlayer)
        {
            case true:
                target = myAI.target;
                range = 0.5f;
                break;
            case false:
                target = myAI.targetHideable.transform.position;
                range = 2.5f;
                break;
        }
        target = myAI.target;
        myAI.nodePrint(this);
#if UNITY_EDITOR
        GameObject obj = GameObject.FindWithTag("Respawn");
        obj.transform.position = target;
#endif
        float dist = Vector3.Distance(target, agent.transform.position);
        if (dist > range)
        {
            agent.isStopped = false;
            agent.SetDestination(target);
            myAI.SetColor(Color.cyan);
            _state = NodeState.RUNNING;
        }
        else
        {
            agent.isStopped = true;
            myAI.canCount = true;
            _state = NodeState.SUCCESS;
        }
        return _state;
    }
}
