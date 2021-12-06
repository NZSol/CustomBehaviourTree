using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HideableSite : BTCoreNode
{
    NavMeshAgent agent;
    GameObject target;
    float range;
    float timer;
    float waitTime;


    public HideableSite(NavMeshAgent agent, TreeFunc myAI, float waitTime)
    {
        this.agent = agent;
        this.myAI = myAI;
        this.waitTime = waitTime;
    }

    public override void onNodeEnter()
    {
        myAI.canRun = true;
    }

    public override NodeState Evaluate()
    {
        onNodeEnter();
        if(myAI.targetHideable == null)
        {
            return NodeState.FAILURE;
        }
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
            timer += Time.deltaTime;
            if (timer > waitTime)
            {
                timer = 0;
                agent.isStopped = true;
                myAI.canCount = true;
                myAI.targetHideable = null;
                _state = NodeState.SUCCESS;
            }
        }
        Debug.Log($"{_state}");
        return _state;
    }
}
