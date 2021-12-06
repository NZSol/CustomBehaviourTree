﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TreeFunc : MonoBehaviour
{
    //----------Detections values----------\\
    [HideInInspector]
    public float sightRange = 0f, chaseRange = 0f, audioValue = 0f;
    [HideInInspector]
    public float waitTimerMin = 0f, waitTimerMax = 5f;
    public bool playerFound = false;
    public bool hidingFound = false;
    //------------Searching Vars------------\\
    [HideInInspector]
    public GameObject targetHideable = null;
    [HideInInspector]
    public float radius = 0;
    //------------Transform Vars------------\\
    [HideInInspector]
    public Transform[] patrolPoints = null;
    [HideInInspector]
    public Transform player = null;
    [HideInInspector]
    public Vector3 target = Vector3.zero;
    [HideInInspector]
    public float fovRange = 0;
    public int patrolValue = 0;
    //------------Accessory Vars------------\\
    [HideInInspector]
    public NavMeshAgent agent = null;
    [HideInInspector]
    public Material mat = null;
    //[HideInInspector]
    public bool canCount = false, canRun = true;
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
        curNode = rootNode;
    }

    float waitTimer(float min, float max)
    {
        float value = Random.Range(min, max);
        return value;
    }

    void ConstructBT()
    {
        //NOTES\\
        /*
         *  Not moving to location of hideable objects
         * 
         * 
         * 
         */


        //----------Initializing Leaves----------\\
        Patrol patrol = new Patrol(patrolValue, patrolPoints, agent, this, waitTimer(waitTimerMin, waitTimerMax));
        PlayerDetect PlayerFinder = new PlayerDetect(this, false, player, agent, fovRange);
        PlayerDetect PlayerFinderInvert = new PlayerDetect(this, true, player, agent, fovRange);
        Chaser chase = new Chaser(player, agent, this);
        PlayerSite lastKnownLocation = new PlayerSite(agent, player, this, waitTimer(waitTimerMin, waitTimerMax));
        HideableSite targetHider = new HideableSite(agent, this, waitTimer(waitTimerMin, waitTimerMax));
        Inverter sightsInvert = new Inverter(PlayerFinderInvert, this);
        FindHideables hideableObjs = new FindHideables(radius, this, agent);

        //-----------Initializing Series-----------\\
        Sequencer playerInSight = new Sequencer(new List<BTCoreNode> { PlayerFinder, new Printer("PlayerFound"), chase }, this, "playerInSight");


        Sequencer patrolSeq = new Sequencer(new List<BTCoreNode> { patrol, new Printer("PatrolSequence") }, this, "PatrolSeq");
        Sequencer searchHiding = new Sequencer(new List<BTCoreNode> { hideableObjs, targetHider, new DisableBool("hidingFound", this)}, this, "SearchHidingSequence");
        Selector searchHiddenLocal = new Selector(new List<BTCoreNode> { searchHiding }, this, "SearchHidden||LocalAreas");
        Sequencer moveToLastKnownLocation = new Sequencer(new List<BTCoreNode> { lastKnownLocation, new DisableBool("playerFound", this), searchHiddenLocal }, this, "MoveToLocationSequence");

        Sequencer lostSightsSeq = new Sequencer(new List<BTCoreNode> { sightsInvert, new BooleanCheck(playerFound, this), moveToLastKnownLocation }, this, "LostSightSeq");
        Selector huntInvestigate = new Selector(new List<BTCoreNode> { playerInSight, lostSightsSeq }, this, "HuntInvestigate");



        rootNode = new Selector(new List<BTCoreNode> { huntInvestigate, patrolSeq }, this, "Root");
    }

    private void Update()
    {
        rootNode.Evaluate();
        if (PlayerInView())
        {
            canCount = false;
        }

        if (rootNode.state == BTCoreNode.NodeState.FAILURE)
        {
            mat.color = Color.black;
        }
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
        //print($"{gameObject.name} {node}");
    }


    bool PlayerInView()
    {
        RaycastHit hit;
        Vector3 rayDir = player.transform.position - agent.transform.position;
        if (Vector3.Angle(rayDir, agent.transform.forward) < fovRange)
        {
            if (Physics.Raycast(agent.transform.position, rayDir, out hit))
            {
                if (hit.transform.tag == "Player")
                {
                    Debug.DrawRay(agent.transform.position, rayDir, Color.green);
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


