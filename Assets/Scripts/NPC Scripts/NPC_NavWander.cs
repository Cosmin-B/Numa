using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_NavWander : MonoBehaviour 
{
    private NPC_Manager npcMaster;
    private NavMeshAgent myNavMeshAgent;
    private Transform myTransform;
    private NavMeshHit navHit;
    private Vector3 wanderTarget;
    private float checkRate;
    private float nextCheck;
    private float wanderRange = 10;

    void OnEnable()
   {
        SetInitialReferences();
        npcMaster.EventNPCDie += DisableThis;
   }

   void OnDisable()
   {
        npcMaster.EventNPCDie -= DisableThis;
    }

   void Update()
   {
        if (Time.time > nextCheck)
        {
            nextCheck = Time.time + checkRate;
            CheckIfShouldWander();
        }
    }

   void SetInitialReferences()
   {
        npcMaster = GetComponent<NPC_Manager>();
        if (GetComponent<NavMeshAgent>() != null)
        {
            myNavMeshAgent = GetComponent<NavMeshAgent>();
        }

        checkRate = Random.Range(0.3f, 0.4f);
        myTransform = transform;
    }

    void CheckIfShouldWander()
    {
        if (npcMaster.myTarget == null && !npcMaster.isOnRoute)
        {
            if(RandomWanderTarget(myTransform.position, wanderRange,out wanderTarget))
            {
                myNavMeshAgent.SetDestination(wanderTarget);
                npcMaster.isOnRoute = true;
                npcMaster.CallEventNPCWalking();
            }
        }
    }

    bool RandomWanderTarget(Vector3 center, float range,out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * wanderRange;
        if(NavMesh.SamplePosition(randomPoint,out navHit,1.0f,NavMesh.AllAreas))
        {
            result = navHit.position;
            return true;
        }
        else
        {
            result = center;
            return false;
        }
    }

    void DisableThis()
    {
        this.enabled = false;
    }
}
