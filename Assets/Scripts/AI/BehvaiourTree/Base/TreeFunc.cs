using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine.Profiling;

public class TreeFunc : MonoBehaviour
{
    //-----------Detections vars-----------\\
    [HideInInspector]
    public float sightRange = 0f, chaseRange = 0f, audioValue = 0f;
    [HideInInspector]
    public float waitTimerMin = 0f, waitTimerMax = 5f;
    public bool playerFound = false;
    public bool hidingFound = false;
    [HideInInspector]
    public float volume, volumeLimit;

    NativeArray<RaycastCommand> rayCommand;
    NativeArray<RaycastHit> rayHit;
    JobHandle handle;

    //------------Searching Vars------------\\
    [HideInInspector]
    public GameObject targetHideable = null;
    [HideInInspector]
    public float radius = 0;
    [HideInInspector]
    public LayerMask layer;
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
    public Queue<Vector3> exploreLocation = new Queue<Vector3>();
    //------------Accessory Vars------------\\
    [HideInInspector]
    public NavMeshAgent agent = null;
    [HideInInspector]
    public Material mat = null;
    //[HideInInspector]
    public bool canCount = false, canRun = true, gotPositions = false;

    public GameObject shape;
    //-------Initialize accessory vars-------\\
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mat = GetComponent<MeshRenderer>().material;

        rayCommand = new NativeArray<RaycastCommand>(1, Allocator.Persistent);
        rayHit = new NativeArray<RaycastHit>(1, Allocator.Persistent);

        Profiler.logFile = "myLog";
        Profiler.enableBinaryLog = true;
        Profiler.enabled = true;
        Profiler.maxUsedMemory = 256 * 1024 * 1024;
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
        GetPositions getPos = new GetPositions(agent, radius, layer, 5, this);
        InvestigateSites moveToPositions = new InvestigateSites(exploreLocation, agent, waitTimer(waitTimerMin, waitTimerMax), this);

        //-----------Initializing Series-----------\\
        Sequencer playerInSight = new Sequencer(new List<BTCoreNode> { PlayerFinder, new Printer("PlayerFound"), chase }, this, "playerInSight");


        Sequencer patrolSeq = new Sequencer(new List<BTCoreNode> { patrol, new Printer("PatrolSequence") }, this, "PatrolSeq");
        Sequencer searchLocal = new Sequencer(new List<BTCoreNode> { getPos, moveToPositions }, this, "LocalSeq");
        Sequencer searchHiding = new Sequencer(new List<BTCoreNode> { hideableObjs, targetHider, new DisableBool("hidingFound", this)}, this, "SearchHidingSequence");
        Selector searchHiddenLocal = new Selector(new List<BTCoreNode> { searchHiding, searchLocal }, this, "SearchHidden||LocalAreas");
        Sequencer moveToLastKnownLocation = new Sequencer(new List<BTCoreNode> { lastKnownLocation, new DisableBool("playerFound", this), searchHiddenLocal }, this, "MoveToLocationSequence");

        Sequencer lostSightsSeq = new Sequencer(new List<BTCoreNode> { sightsInvert, new BooleanCheck(playerFound, this), moveToLastKnownLocation }, this, "LostSightSeq");
        Selector huntInvestigate = new Selector(new List<BTCoreNode> { playerInSight, lostSightsSeq }, this, "HuntInvestigate");



        rootNode = new Selector(new List<BTCoreNode> { huntInvestigate, patrolSeq }, this, "Root");
    }


    private void Update()
    {
        Profiler.BeginSample("Tree");
        rootNode.Evaluate();
        Profiler.EndSample();

        Vector3 rayDir = player.transform.position - agent.transform.position;

        ScheduleCast(rayDir);
        handle.Complete();
        RaycastHit hit = rayHit[0];
        if (CanSeePlayer(hit, rayDir))
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


    void ScheduleCast(Vector3 dir)
    {
        rayCommand[0] = new RaycastCommand(agent.transform.position, dir);
        handle = RaycastCommand.ScheduleBatch(rayCommand, rayHit, 1);
    }

    bool CanSeePlayer(RaycastHit hit, Vector3 dir)
    {
        if (Vector3.Angle(dir, agent.transform.forward) < fovRange)
        {
            if (hit.transform != null && hit.transform.tag == "Player")
            {
                target = hit.point;
                if (canCount)
                {
                    canCount = false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void OnDestroy()
    {
        rootNode.CleanUp();
        rayHit.Dispose();
        rayCommand.Dispose();
        print("Destroy");
    }
}


