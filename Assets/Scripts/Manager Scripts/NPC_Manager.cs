using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Manager : MonoBehaviour
{
    public Transform myTarget;

    public bool isOnRoute;
    public bool isNavPaused;

    public delegate void GeneraEventHandler();
    public event GeneraEventHandler EventNPCDie;
    public event GeneraEventHandler EventNPCWalking;
    public event GeneraEventHandler EventNPCHealthLow;
    public event GeneraEventHandler EventNPCHealthRecovered;
    public event GeneraEventHandler EventNPCLostTarget;

    public delegate void HealthEventHandler(int health);
    public event HealthEventHandler EventNPCDeductHealth;
    public event HealthEventHandler EventNPCIncreaseHealth;

    public delegate void NavTargetEventHandler(Transform targetTransfrom);
    public event NavTargetEventHandler EventNPCSetNavTarget;

    public void CallEventNPCDeductHealth(int health)
    {
        if (EventNPCDeductHealth != null)
        {
            EventNPCDeductHealth(health);
        }
    }

    public void CallEventNPCIncreaseHealth(int health)
    {
        if (EventNPCIncreaseHealth != null)
        {
            EventNPCIncreaseHealth(health);
        }
    }

    public void CallEventNPCSetNavTarget(Transform targetTransform)
    {
        if (EventNPCSetNavTarget != null)
        {
            EventNPCSetNavTarget(targetTransform);
        }

        myTarget = targetTransform;
    }

    public void CallEventEnemyDie()
    {
        if (EventNPCDie != null)
        {
            EventNPCDie();
        }
    }

    public void CallEventNPCWalking()
    {
        if (EventNPCWalking != null)
        {
            EventNPCWalking();
        }
    }

    public void CallEventNPCHealthLow()
    {
        if (EventNPCHealthLow != null)
        {
            EventNPCHealthLow();
        }
    }

    public void CallEventNPCHealthRecovered()
    {
        if (EventNPCHealthRecovered != null)
        {
            EventNPCHealthRecovered();
        }
    }

    public void CallEventNPCLostTarget()
    {
        if(EventNPCLostTarget !=null)
        {
            EventNPCLostTarget();
        }

        myTarget = null;
    }
}
