using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetPositions : BTCoreNode
{
    Queue<Vector3> positions = new Queue<Vector3>();
    NavMeshAgent agent;
    float radius;
    LayerMask layer;


    public GetPositions(NavMeshAgent agent, float radius, LayerMask layer, TreeFunc myAI)
    {
        this.agent = agent;
        this.myAI = myAI;
        this.radius = radius;
        this.layer = layer;
    }
    Vector3 randomNavSphere(Vector3 position, float radius, LayerMask layer)
    {
        Vector3 randomPoint = Random.insideUnitSphere * radius;
        randomPoint += position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, radius, layer);

        return navHit.position;
    }


    public override NodeState Evaluate()
    {
        if(positions.Count < 3)
        {
            for (int i = 0; i < 3; ++i)
            {
                positions.Enqueue(randomNavSphere(agent.transform.position, radius, layer));
            }
            myAI.gotPositions = true;
            return NodeState.SUCCESS;
        }

        if(myAI.gotPositions)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.RUNNING;
    }

}
