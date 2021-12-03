using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HideableSite : BTCoreNode
{
    NavMeshAgent agent;
    Transform player;
    string nodeName;
    GameObject target;
    float range;


    public HideableSite(NavMeshAgent agent, Transform player, TreeFunc myAI)
    {
        this.agent = agent;
        this.player = player;
        this.myAI = myAI;
    }


    public override NodeState Evaluate()
    {
        Debug.Log("Hiding");
        target = myAI.targetHideable;
        range = 0.5f;
        myAI.nodePrint(this);

#if UNITY_EDITOR
        GameObject obj = null;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Respawn");
        foreach(GameObject target in objs)
        {
            if (target.name.Contains("Hideable"))
            {
                obj = target;
            }
        }
        obj.transform.position = target.transform.position;
#endif

        float dist = Vector3.Distance(target.transform.position, agent.transform.position);
        if (dist > range)
        {
            agent.isStopped = false;
            agent.SetDestination(target.transform.position);
            myAI.SetColor(Color.cyan);
            _state = NodeState.RUNNING;
        }
        else
        {
            agent.isStopped = true;
            myAI.canCount = true;
            _state = NodeState.SUCCESS;
        }
        Debug.Log($"{_state} {nodeName}");
        return _state;
    }
}
