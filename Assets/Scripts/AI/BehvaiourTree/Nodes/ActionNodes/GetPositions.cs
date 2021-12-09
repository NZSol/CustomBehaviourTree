using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Jobs;
using Unity.Collections;

public class GetPositions : BTCoreNode
{
    Queue<Vector3> positions = new Queue<Vector3>();
    NavMeshAgent agent;
    float radius;
    LayerMask layer;
    float minDistance;
    JobHandle handle;
    NativeArray<RaycastCommand> rayCommand;
    NativeArray<RaycastHit> rayHit;


    public GetPositions(NavMeshAgent agent, float radius, LayerMask layer, float minDistance, TreeFunc myAI)
    {
        this.agent = agent;
        this.myAI = myAI;
        this.radius = radius;
        this.layer = layer;
        this.minDistance = minDistance;

        rayCommand = new NativeArray<RaycastCommand>(1, Allocator.Persistent);
        rayHit = new NativeArray<RaycastHit>(1, Allocator.Persistent);
    }

    void ScheduleCast(Vector3 rayDir)
    {
        rayCommand[0] = new RaycastCommand(agent.transform.position, rayDir);
        handle = RaycastCommand.ScheduleBatch(rayCommand, rayHit, 30);
    }

    bool randomPoint(Vector3 origin, float radius, out Vector3 result, float minDistance)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = origin + Random.insideUnitSphere * radius;
            ScheduleCast(randomPoint - origin);
            handle.Complete();

            RaycastHit[] hit = rayHit.ToArray();
            foreach (RaycastHit rayOut in hit)
            {
                float dist = Vector3.Distance(rayOut.point, agent.transform.position);
                if (rayOut.transform != null && rayOut.transform.name.Contains("Plane") && dist > minDistance)
                {
                    result = rayOut.point;
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
                if (randomPoint(agent.transform.position, radius, out point, 5))
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

    public override void CleanUp()
    {
        rayHit.Dispose();
        rayCommand.Dispose();
    }
}
