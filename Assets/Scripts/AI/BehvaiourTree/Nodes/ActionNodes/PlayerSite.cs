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
    float timer;
    float waitTime;

    string nodeName;

    public PlayerSite(NavMeshAgent agent, Transform player, TreeFunc myAI, float waitTime)
    {
        this.agent = agent;
        this.player = player;
        this.myAI = myAI;
        this.waitTime = waitTime;
    }


    public override void onNodeEnter()
    {
        base.onNodeEnter();
        myAI.SetColor(Color.white);
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
        if (dist > range && myAI.playerFound)
        {
            agent.isStopped = false;
            agent.SetDestination(target);
            myAI.SetColor(Color.cyan);
            _state = NodeState.RUNNING;
        }
        else
        {
            timer += Time.deltaTime;
            if(timer > waitTime)
            {
                timer = 0;
                myAI.canRun = false;
                agent.isStopped = true;
                myAI.canCount = true;
                _state = NodeState.SUCCESS;
            }
        }
        return _state;
    }
}
