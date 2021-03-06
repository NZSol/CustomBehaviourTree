using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDetect : BTCoreNode
{
    bool invert;
    Transform player;
    NavMeshAgent agent;
    float FOV;

    NativeArray<RaycastCommand> castCommands;
    NativeArray<RaycastHit> castHits;
    JobHandle handle;
    
    public PlayerDetect(TreeFunc myAI, bool invert, Transform player, NavMeshAgent agent, float FOV)
    {
        this.myAI = myAI;
        this.invert = invert;
        this.player = player;
        this.agent = agent;
        this.FOV = FOV;

        castCommands = new NativeArray<RaycastCommand>(1, Allocator.Persistent);
        castHits = new NativeArray<RaycastHit>(1, Allocator.Persistent);
    }


    public override string ToString()
    {
        return "PlayerDetect" + " Inverted == " + invert;
    }

   

    public override NodeState Evaluate()
    {
        Vector3 rayDir = player.transform.position - agent.transform.position;

        ScheduleCast(rayDir);
        handle.Complete();
        RaycastHit raycastHit = castHits[0];
        bool hit = CanSeePlayer(raycastHit, rayDir);



        myAI.nodePrint(this);
        myAI.SetColor(Color.green);
        if (!invert && hit)
        {
            myAI.playerFound = true;
            myAI.hidingFound = true;
        }

        return hit ? NodeState.SUCCESS : NodeState.FAILURE;
    }

    
    void ScheduleCast(Vector3 dir)
    {
        castCommands[0] = new RaycastCommand(agent.transform.position, dir);
        handle = RaycastCommand.ScheduleBatch(castCommands, castHits, 1);
    }

    public bool CanSeePlayer(RaycastHit hit, Vector3 dir)
    {
        if (Vector3.Angle(dir, agent.transform.forward) < FOV)
        {
            if (hit.transform != null && hit.transform.tag == "Player")
            {
                myAI.target = hit.point;
                if (myAI.canCount)
                {
                    myAI.canCount = false;
                }
                //Debug.DrawRay(agent.transform.position, dir, Color.green);
                return true;
            }
            else
            {
                //Debug.DrawRay(agent.transform.position, dir, Color.yellow);
                return false;
            }
        }
        else
        {
            //Debug.DrawRay(agent.transform.position, dir, Color.red);
            return false;
        }
    }
    public override void CleanUp()
    {
        Debug.Log("hit");
        castCommands.Dispose();
        castHits.Dispose();
    }
}
