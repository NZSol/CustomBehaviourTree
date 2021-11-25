using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class newAI : MonoBehaviour
{
    #region variables
    //----------Detections values----------\\
    [SerializeField] float sightRange = 0;
    [SerializeField] float chaseRange = 0;
    [SerializeField] float audioValue = 0;
    [SerializeField] int waitTimer = 0;
    //------------Transform Vars------------\\
    [SerializeField] Transform[] patrolPoints = null;
    [SerializeField] Transform player = null;
    [SerializeField] Transform target = null;
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
        tree.waitTimer = waitTimer;
        tree.fovRange = fovRange;
        
        //------Location Vars------\\
        tree.patrolPoints = patrolPoints;
        tree.player = player;
        tree.target = target;
        tree.patrolValue = patrolValue;
    }
}
