using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NewAI : MonoBehaviour
{
    #region variables
    //----------Detections values----------\\
    [SerializeField] float sightRange = 0;
    [SerializeField] float chaseRange = 0;
    [SerializeField] float audioValue = 0;
    [SerializeField] int waitTimerMin = 0, waitTimerMax = 5;
    //----------Searching values----------\\
    [SerializeField] float radius = 0;
    [SerializeField] LayerMask mask = 1 << 8;
    //------------Transform Vars------------\\
    [SerializeField] Transform[] patrolPoints = null;
    [SerializeField] Transform player = null;
    [SerializeField] float fovRange = 0;
    [SerializeField] int patrolValue = 0;
    //------------Accessory Vars------------\\
    NavMeshAgent agent;
    Material mat;
    #endregion

    //Initialize local vars
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mat = GetComponent<MeshRenderer>().material;
    }

    //Create and send parameters to unique BT
    void Start()
    {
        var tree = gameObject.AddComponent<TreeFunc>();
        SendParameters(tree);
    }

    void SendParameters(TreeFunc tree)
    {
        //-----Detection Vars-----\\
        tree.sightRange = sightRange;
        tree.chaseRange = chaseRange;
        tree.audioValue = audioValue;
        tree.waitTimerMin = waitTimerMin;
        tree.waitTimerMax = waitTimerMax;
        tree.fovRange = fovRange;
        //----------Searching values----------\\
        tree.radius = radius;
        tree.layer = mask;
        //------Location Vars------\\
        tree.patrolPoints = patrolPoints;
        tree.player = player;
        tree.patrolValue = patrolValue;
    }
}
