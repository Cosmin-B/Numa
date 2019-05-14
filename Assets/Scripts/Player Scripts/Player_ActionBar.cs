using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_ActionBar : MonoBehaviour
{

    private Player_StateChanger _stateChanger;
    private Player_Inventory _inventory;
    private HUD_Model _hudModel;

    void Start()
    {
        _inventory = GetComponent<Player_Inventory>();
        Debug.Assert(_inventory != null);

        _stateChanger = GetComponent<Player_StateChanger>();

        Debug.Assert(_stateChanger != null);

        _hudModel = GetComponent<HUD_Model>();
        Debug.Assert(_hudModel != null);
    }

    private void Update()
    {
        GetActionBatInput();
    }

    void GetActionBatInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_inventory.HasItemWithTagInInventory(GameManager_References.wheelsTag))
                _stateChanger.ChangeMovementMode(
                    _stateChanger.playerController.GetMovementType() == Player_Controller.MovementType.MOVE_WHEEL ?
                    Player_Controller.MovementType.MOVE_WALK : Player_Controller.MovementType.MOVE_WHEEL);
            //add transitionsound
            else
            {
                Debug.Log("You haven't unlocked wheels yet");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (_inventory.RemovePowerupFromInventory(true))
            {
                Debug.Log("Battery consume");
                _hudModel.UseConsumable(GameManager_References.batteryTag);
                Game_Manager.Instance.GetSoundManager().PlaySoundForPlayer(Player_SoundHolder.useItemSound);
            }
            else
                Debug.Log("You don't have any battery");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (_inventory.RemovePowerupFromInventory(false))
            {
                Debug.Log("Fuel consumed");
                _hudModel.UseConsumable(GameManager_References.fuelTag);
                Game_Manager.Instance.GetSoundManager().PlaySoundForPlayer(Player_SoundHolder.useItemSound);
            }
            else
                Debug.Log("You don't have any fuel");
        }
    }
}
