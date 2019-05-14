using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_ToggleInventoryUI : MonoBehaviour
{
    [Tooltip("Do we have an invetory? Set to true if YES")]
    public bool hasInvetory;
    public GameObject inventoryUI;
    public string toggleInvetoryButton;
    private Game_Manager gameManagerMaster;

    void Start()
    {
        SetInitialReferences();
    }

    void Update()
    {
        CheckForInvetoryToggle();
    }

    void SetInitialReferences()
    {
        gameManagerMaster = GetComponent<Game_Manager>();
        if(toggleInvetoryButton == "")
        {
            Debug.LogWarning("Type the name of the button for invetory");
            this.enabled = false;
        }
    }

    void CheckForInvetoryToggle()
    {
        if(Input.GetButtonUp(toggleInvetoryButton) && 
                       !gameManagerMaster.isMenuOn && 
                     !gameManagerMaster.isGameOver &&
                     hasInvetory)
        {
            ToggleInvetoryUI();
        }
    }

    public void ToggleInvetoryUI()
    {
        if(inventoryUI !=null)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            gameManagerMaster.isInvetoryUIOn = !gameManagerMaster.isInvetoryUIOn;
            gameManagerMaster.CallEventInventoryUIToggle();
        }
    }

}
