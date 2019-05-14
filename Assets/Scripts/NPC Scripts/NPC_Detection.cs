using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Detection : MonoBehaviour
{
    private NPC_Manager enemyMaster;
    private Transform myTransform;
    private RaycastHit hit;
    public Transform head;
    public LayerMask playerLayer;
    public LayerMask sightLayer;

    private float checkRate;
    private float nextCheck;
    private float detectRadius = 5;

    void OnEnable()
    {
        SetInitialReferences();
        enemyMaster.EventNPCDie += DisableThis;
    }

    void OnDisable()
    {
        enemyMaster.EventNPCDie -= DisableThis;
    }

    void Update()
    {
        CarryOutDetection();
    }

    void SetInitialReferences()
    {
        enemyMaster = GetComponent<NPC_Manager>();
        myTransform = transform;

        if (head == null)
        {
            head = myTransform;
        }

        checkRate = Random.Range(0.8f, 1.2f);
    }

    void CarryOutDetection()
    {
        if (Time.time > nextCheck)
        {
            nextCheck = Time.time + checkRate;

            Collider[] colliders = Physics.OverlapSphere(myTransform.position, detectRadius, playerLayer);

            if (colliders.Length > 0)
            {
                foreach (Collider potentialTargetCollider in colliders)
                {
                    if (potentialTargetCollider.CompareTag(GameManager_References._playerTag))
                    {
                        if (canPotentialTargetBeSeen(potentialTargetCollider.transform))
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                enemyMaster.CallEventNPCLostTarget();
            }
        }
    }

    private bool canPotentialTargetBeSeen(Transform potentianTarget)
    {
        if (Physics.Linecast(head.position, potentianTarget.position, out hit, sightLayer))
        {
            if (hit.transform == potentianTarget)
            {
                enemyMaster.CallEventNPCSetNavTarget(potentianTarget);
                return true;
            }
            else
            {
                enemyMaster.CallEventNPCLostTarget();
                return false;
            }
        }
        else
        {
            enemyMaster.CallEventNPCLostTarget();
            return false;
        }
    }

    void DisableThis()
    {
        this.enabled = false;
    }
}
