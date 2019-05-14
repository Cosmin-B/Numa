using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City_AreaTriggerScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == GameManager_References._playerTag)
        {
            Debug.Log("Player enters city collider");

            HUD_Model hudModel = other.GetComponent<HUD_Model>();

            if (hudModel == null)
                return;

            hudModel.checkPoint.position = transform.position;

            hudModel.StopCoroutine("DrainPowertype");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == GameManager_References._playerTag)
        {
            Debug.Log("Player exits city collider");

            HUD_Model hudModel = other.GetComponent<HUD_Model>();

            if (hudModel == null)
                return;

            hudModel.CanPlayerSurvive();
        }
    }
}
