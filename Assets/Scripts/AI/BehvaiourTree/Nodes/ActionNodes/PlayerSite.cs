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
    float range;

    string nodeName;

    public PlayerSite(NavMeshAgent agent, Transform player, TreeFunc myAI)
    {
        this.agent = agent;
        this.player = player;
        this.myAI = myAI;
    }


    public override void onNodeEnter()
    {
        base.onNodeEnter();
        myAI.SetColor(Color.white);
        Debug.Log(this);
    }

    public override NodeState Evaluate()
    {
        if (myAI.curNode != this)
            onNodeEnter();

        target = myAI.target;
        range = 0.5f;
        myAI.nodePrint(this);

#if UNITY_EDITOR
        GameObject obj = null;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject target in objs)
        {
            if (target.name.Contains("Player"))
                obj = target;
        }
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
        //Debug.Log($"{_state} {nodeName}");
        return _state;
    }
}
