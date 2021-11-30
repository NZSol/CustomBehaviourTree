using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDetect : BTCoreNode
{
    bool invert;
    Transform player;
    NavMeshAgent agent;
    float FOV;
    public PlayerDetect(TreeFunc myAI, bool invert, Transform player, NavMeshAgent agent, float FOV)
    {
        this.myAI = myAI;
        this.invert = invert;
        this.player = player;
        this.agent = agent;
        this.FOV = FOV;
    }

    public override string ToString()
    {
        return "PlayerDetect" + " Inverted == " + invert;
    }

    public override NodeState Evaluate()
    {
        myAI.nodePrint(this);
        myAI.SetColor(Color.green);
        LayerMask mask = 1 << 1;
        Debug.Log(mask);
        if (!invert && CanSeePlayer())
        {
            myAI.playerFound = true;
        }
        return CanSeePlayer() ? NodeState.SUCCESS : NodeState.FAILURE;
    }


    public bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 rayDir = player.transform.position - agent.transform.position;
        if (Vector3.Angle(rayDir, agent.transform.forward) < FOV)
        {
            if (Physics.Raycast(agent.transform.position, rayDir, out hit))
            {
                if (hit.transform.tag == "Player")
                {
                    Debug.DrawRay(agent.transform.position, rayDir, Color.green);
                    myAI.target = hit.point;
                    if (myAI.canCount)
                    {
                        myAI.canCount = false;
                    }
                    return true;
                }
                else
                {
                    Debug.DrawRay(agent.transform.position, rayDir, Color.yellow);
                    return false;
                }
            }
        }
        else
        {
            Debug.DrawRay(agent.transform.position, rayDir, Color.red);
            return false;
        }
        Debug.DrawRay(agent.transform.position, rayDir, Color.black);
        return false;
    }
}
