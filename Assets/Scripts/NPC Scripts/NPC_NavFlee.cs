using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_NavFlee : MonoBehaviour
{
    private NPC_Manager npcMaster;
    private NavMeshAgent myNavMeshAgent;
    private NavMeshHit navHit;
    private Vector3 runPosition;
    private Vector3 directionToPlayer;
    private Transform myTransform;

    public Transform fleeTarget;

    public bool isFleeing;
    public float fleeRange = 10;

    private float checkRate;
    private float nextCheck;

    private float _checkDestinationTimer = 0.7f;
    private float _checkDestintationCurrentTImer = 0.0f;

    private float _stopDistance = 1.0f;

    void OnEnable()
    {
        fleeTarget = GameObject.FindGameObjectWithTag("Player").transform;
        SetInitialReferences();
        npcMaster.EventNPCDie += DisableThis;
        npcMaster.EventNPCSetNavTarget += SetFleeTarget;
        npcMaster.EventNPCHealthLow += IShouldFlee;
        npcMaster.EventNPCHealthRecovered += IShouldStopFleeing;

    }

    void OnDisable()
    {
        fleeTarget = null;
        npcMaster.EventNPCDie -= DisableThis;
        npcMaster.EventNPCSetNavTarget -= SetFleeTarget;
        npcMaster.EventNPCHealthLow -= IShouldFlee;
        npcMaster.EventNPCHealthRecovered -= IShouldStopFleeing;
    }

    void Update()
    {
        if(Time.time > nextCheck)
        {
            nextCheck = Time.time + checkRate;

            CheckIfIShouldFlee();
        }

        if(isFleeing && npcMaster.isOnRoute)
        {
            _checkDestintationCurrentTImer += Time.deltaTime;

            if(_checkDestintationCurrentTImer >= _checkDestinationTimer)
            {
                _checkDestinationTimer = 0.0f;

                if(Vector3.Distance(myTransform.position, runPosition) < _stopDistance)
                {
                    npcMaster.isOnRoute = false;
                }
            }
        }
    }

    void SetInitialReferences()
    {
        npcMaster = GetComponent<NPC_Manager>();
        myTransform = transform;

        if (GetComponent<NavMeshAgent>() != null)
        {
            myNavMeshAgent = GetComponent<NavMeshAgent>();
        }

        checkRate = Random.Range(0.6f, 0.8f);
    }

    void SetFleeTarget(Transform target)
    {
        fleeTarget = target;
    }

    void IShouldFlee()
    {
        isFleeing = true;
   }

    void IShouldStopFleeing()
    {
        isFleeing = false;
    }

    void CheckIfIShouldFlee()
    {
        if(isFleeing)
        {
            if (fleeTarget != null && !npcMaster.isOnRoute && !npcMaster.isNavPaused)
            {
                if (FleeTarget(out runPosition) && Vector3.Distance(myTransform.position, fleeTarget.position) < fleeRange) 
                {
                    myNavMeshAgent.SetDestination(runPosition);
                    npcMaster.CallEventNPCWalking();
                    npcMaster.isOnRoute = true;
                }
            }
        }
    }

    bool FleeTarget(out Vector3 result)
    {
        directionToPlayer = myTransform.position - fleeTarget.position;
        Vector3 checkPosition = myTransform.position + directionToPlayer;

        if(NavMesh.SamplePosition(checkPosition, out navHit,1.0f,NavMesh.AllAreas))
        {
            result = navHit.position;
            return true;
        }
        else
        {
            result = myTransform.position;
            return false;
        }
    }

    void DisableThis()
    {
        this.enabled = false;
    }
}
