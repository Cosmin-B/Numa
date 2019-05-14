using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_HealthA : MonoBehaviour
{
    public int npcHealth = 100;
    private int maxHealth = 100;

    private GameObject _fuelPrefab;
    private GameObject _batteryPrefab;

    private GameObject _fuel;
    private GameObject _battery;

    void Start()
    {
        if (_fuelPrefab == null)
        {
            _fuelPrefab = (GameObject)Resources.Load("Prefabs/Fuel", typeof(GameObject));
        }

        if (_batteryPrefab == null)
        {
            _batteryPrefab = (GameObject)Resources.Load("Prefabs/Battery", typeof(GameObject));
        }
    }

    void Update()
    {
        //RegenerateHealthOverTime();

        if (npcHealth > maxHealth)
        {
            npcHealth = maxHealth;
        }

        if (npcHealth < 0)
        {
            Destroy(this.gameObject);
            _fuel = Instantiate(_fuelPrefab, transform.position, transform.rotation);
            _battery = Instantiate(_batteryPrefab, transform.position, transform.rotation);

            _fuel.GetComponent<Rigidbody>().velocity = _fuel.transform.up * 2;
            _battery.GetComponent<Rigidbody>().velocity = _battery.transform.up * 2;

        }
    }

    private void RegenerateHealthOverTime()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            npcHealth -= 10;
        }
    }
}
