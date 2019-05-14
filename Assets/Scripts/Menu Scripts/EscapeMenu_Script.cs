using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu_Script : MonoBehaviour
{
    public Transform escapeMenu;
    public GameObject collectiblesPanel;
    public GameObject playerObject;
    public GameObject achievementsPanel;
    public GameObject optionsPanel;
    public GameObject canvasInventory;
    public GameObject canvasActipnBar;

    public static GameObject contentAchievements = null;
    public static GameObject canvasAchievementOnScreen = null;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        canvasAchievementOnScreen = GameObject.Find("Canvas_OnScreenAchievement");
        contentAchievements = GameObject.Find("ContentAchievements");

        Debug.Assert(canvasAchievementOnScreen != null);
        Debug.Assert(contentAchievements !=null);

        escapeMenu.gameObject.SetActive(false);
        achievementsPanel.gameObject.SetActive(false);
        optionsPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        collectiblesPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (escapeMenu.gameObject.activeInHierarchy == false)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                Time.timeScale = 0;
                AudioListener.pause = true;

                playerObject.transform.GetComponent<Player_SmoothMouseLook>().enabled = false;

                escapeMenu.gameObject.SetActive(true);
                achievementsPanel.gameObject.SetActive(false);
                optionsPanel.gameObject.SetActive(false);
                collectiblesPanel.gameObject.SetActive(false);
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                AudioListener.pause = false;
                Time.timeScale = 1;

                escapeMenu.gameObject.SetActive(false);
                playerObject.transform.GetComponent<Player_SmoothMouseLook>().enabled = true;
            }
        }
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;

        escapeMenu.gameObject.SetActive(false);
        playerObject.transform.GetComponent<Player_SmoothMouseLook>().enabled = true;
    }

}
