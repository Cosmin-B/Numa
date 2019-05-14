using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interactable : MonoBehaviour 
{
    public LayerMask layerToDetect;
    public LayerMask sightLayer;
    public string buttonInteract;

    private float _checkItemTimer = 0.7f;
    private float _checkItemCurrentTimer = 0.0f;
    private float _detectRadius = 4.0f;
    private float _labelHeight = 100.0f;
    private float _labelWidth = 200.0f;

    private List<GameObject> _itemsToDetect;


    private void Start()
    {
        _itemsToDetect = new List<GameObject>();
       
    }

    void DetectInteractableObjects()
    {
        _checkItemCurrentTimer += Time.deltaTime;

        if (_checkItemCurrentTimer >= _checkItemTimer)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _detectRadius, layerToDetect);
            foreach (Collider collider in colliders)
            {
                if (isInLineOfSight(collider.transform))
                {
                    if (canSeeObject(collider.transform))
                    {
                        ObjectInteractable_Script interactable = collider.gameObject.GetComponent<ObjectInteractable_Script>();

                        if (interactable != null && !interactable.isInteracted)
                            if (!_itemsToDetect.Contains(collider.gameObject))
                                _itemsToDetect.Add(collider.gameObject);
                    }
                }
            }

            if (_itemsToDetect.Count == 0)
                return;

            List<GameObject> itemsToBeDeleted = new List<GameObject>();

            /// Check the items in the list after the player is not within detect radius
            foreach (GameObject item in _itemsToDetect)
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
        DetectInteractableObjects();
        CheckPlayerInput();
    }

    private void OnGUI()
    {
        if (_itemsToDetect != null)
        {
            if (_itemsToDetect.Count > 0)
            {
                GUI.Label(new Rect(Screen.width / 2.0f - _labelHeight / 2.0f, Screen.height / 2.0f, _labelWidth, _labelHeight), "Press E To Interact");
            }
        }
    }

    void CheckPlayerInput()
    {
        if (_itemsToDetect != null && _itemsToDetect.Count > 0)
        {
            if (Input.GetButtonDown(buttonInteract) && Time.timeScale > 0)
            {
                //interact sound
                List<GameObject> itemsToInteract = new List<GameObject>();

                foreach (GameObject item in _itemsToDetect)
                {
                    itemsToInteract.Add(item);
                }

                foreach (GameObject interactableObject in itemsToInteract)
                {
                    ObjectInteractable_Script interactable = interactableObject.GetComponent<ObjectInteractable_Script>();

                    if(interactable != null)
                    {
                        interactable.Interact();
                        _itemsToDetect.Remove(interactableObject);
                    }
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

    private bool canSeeObject(Transform target)
    {
        float angle = 45.0f;
        if (Vector3.Angle(transform.forward, target.position - transform.position) <= angle)
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
