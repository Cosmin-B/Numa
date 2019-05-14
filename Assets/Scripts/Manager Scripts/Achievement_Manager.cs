using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement_Manager : MonoBehaviour
{
    public GameObject achievementPrefab;
    public Sprite[] sprites;
    public GameObject onScreenAchievement;
    public Dictionary<string, Achievements_Scripts> achievementsDictionary = new Dictionary<string, Achievements_Scripts>();
    public Sprite unlockedAchievementSprite;
    public Text pointsText;

    private Player_Inventory _playerInventory;

    private static Achievement_Manager instance;

    public static Achievement_Manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Achievement_Manager>();
            }
            return instance;
        }
    }

    void Start()
    {

        CreateAchievement("ContentAchievements", "Collect Laser Gun", "Congrats ! You have collected the laser gun", 5, 2);
        CreateAchievement("ContentAchievements", "Collect Wheels", "Congrats ! You have collected wheels", 5, 2);
        CreateAchievement("ContentAchievements", "Collect Springs", "Congrats ! You have collected springs", 5, 2);
        CreateAchievement("ContentAchievements", "Collect All Powerups", "Congrats ! You have collected all the objects ", 5, 1, new string[] { "Collect Laser Gun", "Collect Wheels", "Collect Springs" });

        CreateAchievement("ContentAchievements", "Collect Battery", "Congrats ! You have collected battery", 5, 2);
        CreateAchievement("ContentAchievements", "Collect Fuel", "Congrats ! You have collected fuel", 5, 2);

        _playerInventory = GameObject.FindObjectOfType<Player_Inventory>();

        ///THIS SHOULD BE REMOVED
        PlayerPrefs.DeleteAll();
    }

    void Update()
    {

        if (_playerInventory.laserGunCollected == true)
        {
            EarnAchievement("Collect Laser Gun");
        }

        if (_playerInventory.wheelsCollected == true)
        {
            EarnAchievement("Collect Wheels");
        }

        if (_playerInventory.springsCollected == true)
        {
            EarnAchievement("Collect Springs");
        }


        if (_playerInventory.batteryCollected == true)
        {
            EarnAchievement("Collect Battery");
        }

        if (_playerInventory.fuelCollected == true)
        {
            EarnAchievement("Collect Fuel");
        }


    }

    public void EarnAchievement(string title)
    {
        if (achievementsDictionary[title].EarnAchievement())
        {
            GameObject acievementInstantiate = (GameObject)Instantiate(onScreenAchievement);
            SetAchievementInfo(EscapeMenu_Script.canvasAchievementOnScreen, acievementInstantiate, title);
            pointsText.text = "Points : " + PlayerPrefs.GetInt("Points");
            StartCoroutine(HideAchievement(acievementInstantiate));
        }
    }

    public IEnumerator HideAchievement(GameObject achievementToBeDestroyed)
    {
        yield return new WaitForSeconds(3);
        Destroy(achievementToBeDestroyed);
    }

    public void CreateAchievement(string parent, string title, string description, int points, int spriteIndex, string[] dependencies = null)
    {
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);
        Achievements_Scripts newAchievement = new Achievements_Scripts(name, description, points, spriteIndex, achievement);

        achievementsDictionary.Add(title, newAchievement);

        SetAchievementInfo(EscapeMenu_Script.contentAchievements, achievement, title);

        if (dependencies != null)
        {
            foreach (string achievementTitle in dependencies)
            {
                Achievements_Scripts dependency = achievementsDictionary[achievementTitle];
                dependency.Child = title;
                newAchievement.AddDependency(dependency);
            }
        }
    }

    public void SetAchievementInfo(GameObject parent, GameObject achievement, string title)
    {
        Debug.Assert(parent != null);

        achievement.transform.SetParent(parent.transform);
        achievement.transform.localScale = new Vector3(1, 1, 1);
        achievement.transform.GetChild(0).GetComponent<Text>().text = title;
        achievement.transform.GetChild(1).GetComponent<Text>().text = achievementsDictionary[title].Description;
        achievement.transform.GetChild(2).GetComponent<Text>().text = achievementsDictionary[title].Points.ToString();
        achievement.transform.GetChild(3).GetComponent<Image>().sprite = sprites[achievementsDictionary[title].SpriteIndex];
    }
}
