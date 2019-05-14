using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SaveableObject : SaveableObject
{
    private Player_Inventory _inventory;

    private enum PlayerReadSaveDataPosition
    {
        /// <summary>
        /// see SavebleObject ReadSaveDataPosition enum and start the first enum with the last index from there + 1
        /// </summary>
        BATTERY_COUNTER = 4,
        WHEELS = 5,
        SPRINGS = 6,
        GUN = 7,
    }

    public void OnEnable()
    {
        _inventory = GetComponent<Player_Inventory>();
        Debug.Assert(_inventory != null);
    }

    public override void Save(int id)
    {
        saveSpecificData = 
            _inventory.batteryCounter.ToString() + "_" +
            _inventory.HasItemWithTagInInventory(GameManager_References.wheelsTag).ToString() + "_" +
            _inventory.HasItemWithTagInInventory(GameManager_References.springsTag).ToString() + "_" +
            _inventory.HasItemWithTagInInventory(GameManager_References.gunTag).ToString();

        base.Save(id);
    }

    public override void Load(string[] values)
    {
        base.Load(values);

        _inventory.batteryCounter = int.Parse(values[(int)PlayerReadSaveDataPosition.BATTERY_COUNTER]);
        _inventory.hasWheels = bool.Parse(values[(int)PlayerReadSaveDataPosition.WHEELS]);
        _inventory.hasSprings = bool.Parse(values[(int)PlayerReadSaveDataPosition.SPRINGS]);
        _inventory.hasLaserGun = bool.Parse(values[(int)PlayerReadSaveDataPosition.GUN]);
    }
}
