using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    public delegate void GeneralEventHandler();
    public event GeneralEventHandler EventInvetoryChanged;
    public event GeneralEventHandler EventHandsEmpty;

    public delegate void PlayerHealthEventHandler(int healthChange);
    public event PlayerHealthEventHandler EventPlayerHealthDeduction;
    public event PlayerHealthEventHandler EventPlayerHealthIncrease;

    public void CallEventInventoryChange()
    {
        if (EventInvetoryChanged != null)
        {
            EventInvetoryChanged();
        }
    }

    public void CallEventHandsEmpty()
    {
        if (EventHandsEmpty != null)
        {
            EventHandsEmpty();
        }
    }

    public void CallEventPlayerHealthDeduction(int damage)
    {
        if (EventPlayerHealthDeduction != null)
        {
            EventPlayerHealthDeduction(damage);
        }

    }

    public void CallEventPlayerHealthIncrease(int increase)
    {
        if(EventPlayerHealthIncrease !=null)
        {
            EventPlayerHealthIncrease(increase);
        }
    }
}
