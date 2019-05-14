using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Manager : MonoBehaviour
{
    private Player_Manager playerMaster;
    public delegate void GeneralEventHandler();
    public event GeneralEventHandler EventObjectThrow;
    public event GeneralEventHandler EventObjectPickUp;

    public delegate void PickUpActionEventHandler(Transform item);
    public event PickUpActionEventHandler EventPickUpAction;


    void OnEnable()
    {
        SetInitialReferences();
    }

    void SetInitialReferences()
    {
        if (GameManager_References.player != null)
        {
            playerMaster = GameManager_References.player.GetComponent<Player_Manager>();
        }
    }

    public void CallEventObjcetTrow()
    {
        if (EventObjectThrow != null)
        {
            EventObjectThrow();
        }
        playerMaster.CallEventHandsEmpty();
        playerMaster.CallEventInventoryChange();
    }

    public void CallEventObjectPickUp()
    {
        if (EventObjectPickUp != null)
        {
            EventObjectPickUp();
        }
        playerMaster.CallEventInventoryChange();
    }

    public void CallEventPickUpAction(Transform item)
    {
        if(EventPickUpAction != null)
        {
            EventPickUpAction(item);
        }
    }
}

