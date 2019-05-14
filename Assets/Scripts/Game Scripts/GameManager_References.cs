using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_References : MonoBehaviour {

    public string playerTag = "Player" ;
    public static string _playerTag;

    public string npcTag = "Npc";
    public static string _enemyTag;

    public static string batteryTag = "Battery";
    public static string fuelTag = "Fuel";
    public static string wheelsTag = "Wheels";
    public static string legsTag = "Legs";
    public static string gunTag = "LaserGun";
    public static string springsTag = "Springs";

    public static GameObject player;

    private void OnEnable()
    {
        if(playerTag =="")
        {
            Debug.LogWarning("Type player tag");
        }

        if (npcTag == "")
        {
            Debug.LogWarning("Type npc tag");
        }

        _playerTag = playerTag;
        _enemyTag = npcTag;

        player = GameObject.FindGameObjectWithTag(_playerTag);

        Debug.Assert(player != null);
    }
}
