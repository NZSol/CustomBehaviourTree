using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindHideables : BTCoreNode
{
    float radius;
    NavMeshAgent agent;

    public FindHideables(float radius, TreeFunc myAI, NavMeshAgent agent)
    {
        this.radius = radius;
        this.myAI = myAI;
        this.agent = agent;
    }

    List<Collider> locateHideables()
    {
        List <Collider> hitCols = new List<Collider>();
        foreach (var col in Physics.OverlapSphere(agent.transform.position, radius))
        {
            if(col.tag == "hideable")
            {
                hitCols.Add(col);
            }
        }
        if (hitCols.Count != 0)
            return hitCols;
        else
            return null;
    }

    public override NodeState Evaluate()
    {
        Collider[] hide = locateHideables().ToArray();
        if (hide.Length != 0)
            myAI.targetHideable = hide[Random.Range(0, hide.Length - 1)].gameObject;

        if (myAI.targetHideable == null)
            _state = NodeState.FAILURE;
        else
            _state = NodeState.SUCCESS;
        return _state;
    }
}
