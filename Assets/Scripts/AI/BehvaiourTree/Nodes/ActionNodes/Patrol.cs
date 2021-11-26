using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : BTCoreNode
{
    int patrolValue;
    Transform[] patrolPoints;
    NavMeshAgent agent;
    float timer = 0;
    float waitTimer;

    [SerializeField] float myDist = 0;

    public Patrol(int patrolValue, Transform[] patrolPoints, NavMeshAgent agent, TreeFunc myAI, float waitTimer)
    {
        this.patrolValue = patrolValue;
        this.patrolPoints = patrolPoints;
        this.agent = agent;
        this.myAI = myAI;
        this.waitTimer = waitTimer;
        timer = 0;
    }


    public override NodeState Evaluate()
    {
        myAI.nodePrint(this);
        myAI.curNode = this;
        myAI.SetColor(Color.yellow);
        float dist = Vector3.Distance(patrolPoints[myAI.patrolValue].position, agent.transform.position);
        myDist = dist;
        if (dist > 0.5f)
        {
            agent.isStopped = false;
            agent.SetDestination(patrolPoints[myAI.patrolValue].position);
            return NodeState.RUNNING;
        }
        else
        {
            agent.isStopped = true;
            timer += Time.deltaTime;

            if(timer > waitTimer)
            {
                if (patrolValue < patrolPoints.Length - 1)
                {
                    myAI.SetPatrolValue(++patrolValue);
                }
                else
                {
                    patrolValue = 0;
                    myAI.SetPatrolValue(0);
                    
                }
                timer = 0;
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.RUNNING;
            }
            
        }
    }

}
