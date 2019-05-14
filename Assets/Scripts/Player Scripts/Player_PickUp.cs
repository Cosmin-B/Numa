using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player_PickUp : MonoBehaviour
{
    public LayerMask layerToDetect;
    public LayerMask sightLayer;
    public string buttonPickUp;

    private float _checkItemTimer = 0.7f;
    private float _checkItemCurrentTimer = 0.0f;
    private float _detectRadius = 4.0f;
    private float _labelHeight = 100.0f;
    private float _labelWidth = 200.0f;

    private List<GameObject> _itemsToDetect;
    private Player_Inventory _playerInventory;
    private Player_ActionBar _playerActionBar;

    private void Start()
    {
        _itemsToDetect = new List<GameObject>();
        _playerInventory = GetComponent<Player_Inventory>();
        _playerActionBar = GetComponent<Player_ActionBar>();

        Debug.Assert(_playerActionBar != null);
        Debug.Assert(_playerInventory != null);
    }

    void DetectItems()
    {
        _checkItemCurrentTimer += Time.deltaTime;

        if (_checkItemCurrentTimer >= _checkItemTimer)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _detectRadius, layerToDetect);
            foreach (Collider collider in colliders)
            {
                if (isInLineOfSight(collider.transform))
                {
                    if (canSeeItem(collider.transform))
                    {
                        if (!_itemsToDetect.Contains(collider.gameObject))
                            _itemsToDetect.Add(collider.gameObject);
                    }
                }
            }

            if (_itemsToDetect.Count == 0)
                return;

            List<GameObject> itemsToBeDeleted = new List<GameObject>();

            /// Check the items in the list after the player is not within detect radius
            foreach(GameObject item in _itemsToDetect)
            {
                if (Vector3.Distance(transform.position, item.transform.position) > _detectRadius)
                    itemsToBeDeleted.Add(item);
            }

            foreach (GameObject item in itemsToBeDeleted)
                _itemsToDetect.Remove(item);
        }
    }

    public void Update()
    {
        DetectItems();
        CheckPlayerInput();
    }

    private void OnGUI()
    {
        if (_itemsToDetect != null)
        {
            if (_itemsToDetect.Count > 0)
            {
                GUI.Label(new Rect(Screen.width / 2.0f - _labelHeight / 2.0f, Screen.height / 2.0f, _labelWidth, _labelHeight), "Press E To Pick Up");
            }
        }
    }

    void CheckPlayerInput()
    {
        if (_itemsToDetect != null && _itemsToDetect.Count > 0)
        {
            if (Input.GetButtonDown(buttonPickUp) && Time.timeScale > 0)
            {
                Player_Controller.animator.Play("Grab/Push");

                List<GameObject> itemsToBePickedUp = new List<GameObject>();

                foreach(GameObject item in _itemsToDetect)
                {
                    itemsToBePickedUp.Add(item);
                }

                foreach(GameObject itemToBeCollected in itemsToBePickedUp)
                {
                    _itemsToDetect.Remove(itemToBeCollected);
                    _playerInventory.AddItemToInventory(itemToBeCollected);
                }
            }
        }
    }
    private bool isInLineOfSight(Transform target)
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, target.position, out hit, sightLayer))
        {
            return true;
        }
        else
        {
            if (_itemsToDetect != null && _itemsToDetect.Count > 0)
            {

                if (_itemsToDetect.Contains(target.gameObject))
                {
                    _itemsToDetect.Remove(target.gameObject);
                }
            }
        }

        return false;
    }

    private bool canSeeItem(Transform target)
    {
        float angle = 45.0f;
        if (Vector3.Angle(transform.forward, target.position - transform.position ) <= angle)
        {
            return true;
        }
        else
        {
            if (_itemsToDetect != null && _itemsToDetect.Count > 0)
            {
                if (_itemsToDetect.Contains(target.gameObject))
                {
                    _itemsToDetect.Remove(target.gameObject);
                }
            }
        }

        return false;
    }
}
