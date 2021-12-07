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

    bool randomPoint(Vector3 origin, float radius, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = origin + Random.insideUnitSphere * radius;

            RaycastHit hit;
            if (Physics.Raycast(origin, randomPoint - origin, out hit, Mathf.Infinity, layer))
            {
                if (hit.transform.name.Contains("Plane"))
                {
                    result = hit.point;
                    return true;
                }
            }
        }
        result = Vector3.zero;
        return false;
    }



    public override NodeState Evaluate()
    {
        onNodeEnter();
        if(myAI.exploreLocation.Count < 4 && !myAI.gotPositions)
        {
            while (myAI.exploreLocation.Count < 4)
            {
                Vector3 point;
                if (randomPoint(agent.transform.position, radius, out point))
                {
                    myAI.exploreLocation.Enqueue(point);
                    Debug.Log("hit");
                    myAI.exploreLocation = positions;
                }
            }
            Debug.Log(myAI.exploreLocation.Count);
            myAI.gotPositions = true;
            return NodeState.SUCCESS;
        }

        if(myAI.gotPositions && myAI.exploreLocation.Count > 0)
        {
            Debug.Log(myAI.exploreLocation.Count);
            return NodeState.SUCCESS;
        }
        else if (myAI.gotPositions && myAI.exploreLocation.Count == 0)
        {
            myAI.hidingFound = false;
            myAI.gotPositions = false;
            return NodeState.FAILURE;
        }
        return NodeState.RUNNING;
    }

}
