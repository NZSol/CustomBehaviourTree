using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InvestigateSites : BTCoreNode
{
    Queue<Vector3> positions = new Queue<Vector3>();
    Vector3 target;
    int posCount;
    NavMeshAgent agent;
    float timer;
    float waitTime;
    float range = 0.8f;
    GameObject shape;

    public InvestigateSites(Queue<Vector3> positions, NavMeshAgent agent, float waitTime, TreeFunc myAI)
    {
        this.positions = positions;
        this.agent = agent;
        this.waitTime = waitTime;
        this.myAI = myAI;
    }
    public override void onNodeEnter()
    {
#if UNITY_EDITOR
        shape = GameObject.FindWithTag("EditorOnly");
#endif
    }

    void getPos()
    {
        target = myAI.exploreLocation.Dequeue();
        shape.transform.position = target;
    }

    public override NodeState Evaluate()
    {
        onNodeEnter();
        if (target == Vector3.zero)
        {
            getPos();
        }

        _state = NodeState.RUNNING;
        float dist = Vector3.Distance(target, agent.transform.position);

        if(dist > range)
        {
            Debug.Log("Out of range");
            agent.isStopped = false;
            agent.SetDestination(target);
        }
        else
        {
            Debug.Log("In Range");
            agent.isStopped = true;
            timer += Time.deltaTime;
            Debug.Log($"My timer is {timer} of {waitTime}. The number of targets = {myAI.exploreLocation.Count}");
            if (timer > waitTime)
            {
                timer -= waitTime;
                if(myAI.exploreLocation.Count > 0)
                {
                    Debug.Log("hit");
                    getPos();
                    _state = NodeState.RUNNING;
                    return _state;
                }
                else
                {
                    _state = NodeState.FAILURE;
                    return _state;
                }
            }
        }
        return _state;
    }

}