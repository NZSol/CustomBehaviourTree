using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TreeFunc : MonoBehaviour
{
    //----------Detections values----------\\
    [HideInInspector]
    public float sightRange = 0f, chaseRange = 0f, audioValue = 0f, waitTimer = 0f;
    public bool playerFound = false;
    //------------Transform Vars------------\\
    [HideInInspector]
    public Transform[] patrolPoints = null;
    [HideInInspector]
    public Transform player = null, target = null;
    [HideInInspector]
    public float fovRange = 0;
    public int patrolValue = 0;
    //------------Accessory Vars------------\\
    [HideInInspector]
    public NavMeshAgent agent = null;
    [HideInInspector]
    public Material mat = null;
    //-------Initialize accessory vars-------\\
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mat = GetComponent<MeshRenderer>().material;
    }

    //---------------Node Vars---------------\\
    public BTCoreNode curNode;
    private BTCoreNode rootNode;

    private void Start()
    {
        ConstructBT();
    }

    void ConstructBT()
    {
        //NOTES\\
        /*  Patrol overriding tracking
         * 
         * 
         * 
         */


        //----------Initializing Leaves----------\\
        Patrol patrol = new Patrol(patrolValue, patrolPoints, agent, this, waitTimer);
        PlayerDetect PlayerFinder = new PlayerDetect(this, false);
        Chaser chase = new Chaser(player, agent, this);
        PlayerSite lastKnownLocation = new PlayerSite(agent, player, this);
        Inverter sightsInvert = new Inverter(new PlayerDetect(this, true), this);

        //-----------Initializing Series-----------\\
        Sequencer playerInSight = new Sequencer(new List<BTCoreNode> { PlayerFinder, new Printer("PlayerFound"), chase }, this, "playerInSight");


        Sequencer PatrolSeq = new Sequencer(new List<BTCoreNode> { patrol, new Printer("PatrolSequence") }, this, "PatrolSeq");
        Sequencer lostSightsSeq = new Sequencer(new List<BTCoreNode> { sightsInvert, new BooleanCheck(playerFound, this), lastKnownLocation }, this, "LostSightSeq");
        Selector huntInvestigate = new Selector(new List<BTCoreNode> { playerInSight, lostSightsSeq }, this, "HuntInvestigate");



        rootNode = new Selector(new List<BTCoreNode> { huntInvestigate, PatrolSeq }, this, "Root");
    }

    private void Update()
    {
        rootNode.Evaluate();

        if (rootNode.state == BTCoreNode.NodeState.FAILURE)
        {
            mat.color = Color.black;
        }
    }

    public bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 rayDir = player.transform.position - transform.position;
        if (Vector3.Angle(rayDir, transform.forward) < fovRange)
        {
            if (Physics.Raycast(transform.position, rayDir, out hit))
            {
                if (hit.transform.tag == "Player")
                {
                    Debug.DrawRay(transform.position, rayDir, Color.green);
                    return true;
                }
                else
                {
                    Debug.DrawRay(transform.position, rayDir, Color.yellow);
                    return false;
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, rayDir, Color.red);
            return false;
        }
        Debug.DrawRay(transform.position, rayDir, Color.black);
        return false;
    }

    public void SetColor(Color color)
    {
        mat.color = color;
    }
    public void SetPatrolValue(int val)
    {
        patrolValue = val;
    }
    public void nodePrint(BTCoreNode node)
    {
        print(gameObject.name + " " + node);
    }
    public void nodePrint(string node)
    {
        print(gameObject.name + " " + node);
    }
    public void nodePrint(BTCoreNode node, bool invert)
    {
        print(gameObject.name + " " + node + " ivnerted == " + invert);
    }

    public void nodePrint(params object[] data)
    {
        switch (data.Length)
        {
            case 1:
                print(gameObject.name + " " + data[0]);
                break;
            case 2:
                print(gameObject.name + " " + data[0] + " inverted == " + data[1]);
                break;
        }
    }
}


