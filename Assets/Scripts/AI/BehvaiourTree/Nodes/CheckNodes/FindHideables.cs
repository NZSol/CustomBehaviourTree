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
        if (hitcols.Length > 0)
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
        else
            return null;
    }

    public override NodeState Evaluate()
    {
        myAI.nodePrint(this);
        Collider[] hideables = locateHideables();
        if (hideables.Length != 0 && myAI.targetHideable == null)
        {
            myAI.targetHideable = hideables[Random.Range(0, hideables.Length - 1)].gameObject;
            Debug.Log($"My hideable is: {myAI.targetHideable}");
        }

        if (myAI.targetHideable == null)
        {
            _state = NodeState.FAILURE;
            Debug.Log("fail");
        }
        else
        {
            _state = NodeState.SUCCESS;
            Debug.Log("success");
        }
        return _state;
    }
}
