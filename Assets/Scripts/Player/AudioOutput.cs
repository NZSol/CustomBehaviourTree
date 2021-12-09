using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;


public class AudioOutput : MonoBehaviour
{
    public float volume;

    [SerializeField] AnimationCurve audioFalloff;

    Collider[] overlapCols;

    private void Awake()
    {
        overlapCols = new Collider[] { };
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ScheduleTask();
        GetHostilesInArea();
    }

    void ScheduleTask(float radius)
    {

    }

    Collider[] GetHostilesInArea()
    {
        //Collider[] hitCols = Physics.OverlapSphere
        return null;
    }

}
