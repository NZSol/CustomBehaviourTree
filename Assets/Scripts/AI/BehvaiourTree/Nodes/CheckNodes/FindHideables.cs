using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindHideables : BTCoreNode
{
    float radius;
    NavMeshAgent agent;
    Collider[] objects = new Collider[0];

    public FindHideables(float radius, TreeFunc myAI, NavMeshAgent agent)
    {
        this.radius = radius;
        this.myAI = myAI;
        this.agent = agent;
    }

    Collider[] locateHideables()
    {
        List<Collider> finalColsOut = new List<Collider>();
        Collider[] hitcols = Physics.OverlapSphere(agent.transform.position, radius);
        if (hitcols == null)
        {
            return null;
        }
        else
        {
            foreach (Collider col in hitcols)
            {
                if (col.tag == "Hideable")
                {
                    finalColsOut.Add(col);
                }
            }
            hitcols = finalColsOut.ToArray();
            return hitcols;
        }
    }


    public override NodeState Evaluate()
    {
        myAI.nodePrint(this);
        Collider[] hideables = locateHideables();
        if(myAI.targetHideable == null)
        {
            if (hideables.Length == 0)
            {
                myAI.targetHideable = null;
                return NodeState.FAILURE;
            }
            else
            {
                myAI.targetHideable = hideables[Random.Range(0, hideables.Length - 1)].gameObject;
                Debug.Log($"My hideable is: {myAI.targetHideable}");
            }
        }
        _state = NodeState.SUCCESS;
        return _state;
    }
}
