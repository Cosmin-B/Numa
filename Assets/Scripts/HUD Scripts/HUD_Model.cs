using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Model : MonoBehaviour
{
    [Header("Player Settings")]
    public float maxEnergy = 100;
    public float maxFuel = 100;
    public float currentEnergy = 100;
    public float currentFuel = 100;
    public float energyDrainPeriodicTimer = 1.0f;
    public float energyDrainPeriodicAmount = 1.0f;
    public float respawnTimer = 5.0f;
    public float energyDrainAmount = 15.0f;
    public float powerDrainTimer = 1.0f;

    [Header("Pickup Settings")]
    public float energyRestored = 15;
    public float fuelRestored = 15;

    [Header("HUD Settings")]
    public float updateHUDTimer = 0.7f;
    public float updateHudSmoothMultiplier = 5.0f;
    public bool updateHUD = true;

    public delegate void HUDViewUpdate(PowerTypes valueType, float targetValue, bool stopCoroutine = false);
    public event HUDViewUpdate HudUpdate;

    private bool _isAlive = true;

    public Player_CheckPoint checkPoint;

    public enum PowerTypes
    {
        POWER_FUEL = 1,
        POWER_ENERGY = 2,
    }

    public void Start()
    {
        checkPoint = GetComponent<Player_CheckPoint>();

        checkPoint.position = transform.position;
        StartCoroutine(DrainEnergyPeriodically());
    }

    public bool CanPlayerSurvive()
    {
        Player_Inventory inventory = GetComponent<Player_Inventory>();

        if (inventory != null)
        {
            if (!inventory.HasItemWithTagInInventory(GameManager_References.wheelsTag))
            {
                StartCoroutine(ModifyPower(PowerTypes.POWER_ENERGY, -energyDrainAmount, powerDrainTimer));
                return false;
            }
        }

        return true;
    }

    public void UseConsumable(string consumableTag)
    {
        if (consumableTag == GameManager_References.batteryTag)
        {
            currentEnergy += energyRestored;
            
            if (currentEnergy > maxEnergy)
                currentEnergy = maxEnergy;

            if (HudUpdate != null)
                HudUpdate(PowerTypes.POWER_ENERGY, currentEnergy);
        }
        else if (consumableTag == GameManager_References.fuelTag)
        {
            currentFuel += fuelRestored;

            if (currentFuel > maxFuel)
                currentFuel = maxFuel;

            if (HudUpdate != null)
                HudUpdate(PowerTypes.POWER_FUEL, currentFuel);
        }
    }

    IEnumerator RespawnPlayer()
    {
        if (checkPoint.position != Vector3.zero)
        {
            Player_Controller controller = GetComponent<Player_Controller>();

            controller.enabled = false;

            StopCoroutine(DrainEnergyPeriodically());

            yield return new WaitForSeconds(respawnTimer);

            _isAlive = true;
            currentEnergy = maxEnergy;
            transform.position = checkPoint.position;

            if (HudUpdate != null)
                HudUpdate(PowerTypes.POWER_ENERGY, currentEnergy);

            StartCoroutine(DrainEnergyPeriodically());

            controller.enabled = true;
        }
    }

    public IEnumerator DrainEnergyPeriodically()
    {
        yield return new WaitForSeconds(1.5f); 

        while (_isAlive)
        {
            currentEnergy -= energyDrainPeriodicAmount;

            if (currentEnergy <= 0)
            {
                _isAlive = false;
                currentEnergy = 0;
                break;
            }

            if (HudUpdate != null)
                HudUpdate(PowerTypes.POWER_ENERGY, currentEnergy);

            yield return new WaitForSeconds(energyDrainPeriodicTimer);
        }

        if (HudUpdate != null)
            HudUpdate(PowerTypes.POWER_ENERGY, currentEnergy, true);

        if (!_isAlive)
            StartCoroutine(RespawnPlayer());
    }

    public IEnumerator ModifyPower(PowerTypes powerType, float value, float periodicTImer)
    {
        if(powerType == PowerTypes.POWER_ENERGY)
        {
            while(_isAlive)
            {
                currentEnergy += value;

                if (currentEnergy <= 0)
                {
                    _isAlive = false;
                    currentEnergy = 0;
                }

                if (HudUpdate != null)
                    HudUpdate(powerType, currentEnergy);

                yield return new WaitForSeconds(periodicTImer);
            }
        }
    }
}