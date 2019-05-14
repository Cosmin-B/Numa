using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievements_Scripts
{
    private string _name;
    private string _description;
    private string _child;

    private bool isUnlocked;

    private int _points;
    private int _spriteIndex;

    private GameObject _achievementReference;
    private List<Achievements_Scripts> dependencies = new List<Achievements_Scripts>();

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }

    public bool IsUnlocked
    {
        get { return isUnlocked; }
        set { isUnlocked = value; }
    }

    public int Points
    {
        get { return _points; }
        set { _points = value; }
    }

    public int SpriteIndex
    {
        get { return _spriteIndex; }
        set { _spriteIndex = value; }
    }

    public string Child
    {
        get { return _child; }
        set { _child = value; }
    }

    public Achievements_Scripts(string name, string description, int points, int spriteIndex, GameObject achievementReference)
    {
        this._name = name;
        this._description = description;
        this._points = points;
        this._spriteIndex = spriteIndex;
        this._achievementReference = achievementReference;
        this.isUnlocked = false;
        LoadAchievement();
    }

    public void AddDependency(Achievements_Scripts dependency)
    {
        dependencies.Add(dependency);
    }

    public bool EarnAchievement()
    {
        if (!isUnlocked && !dependencies.Exists(x => x.isUnlocked == false))
        {
            _achievementReference.GetComponent<Image>().sprite = Achievement_Manager.Instance.unlockedAchievementSprite;
            SaveAchievement(true);

            if (_child != null)
            {
                Achievement_Manager.Instance.EarnAchievement(_child);
            }

            return true;
        }
        return false;
    }

    public void SaveAchievement(bool value)
    {
        isUnlocked = value;

        int temporaryPoints = PlayerPrefs.GetInt("Points");
        PlayerPrefs.SetInt("Points", temporaryPoints += _points);
        PlayerPrefs.SetInt(_name, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadAchievement()
    {
        isUnlocked = PlayerPrefs.GetInt(_name) == 1 ? true : false;

        if (isUnlocked)
        {
            Achievement_Manager.Instance.pointsText.text = "Points : " + PlayerPrefs.GetInt("Points");
            _achievementReference.GetComponent<Image>().sprite = Achievement_Manager.Instance.unlockedAchievementSprite;
        }
    }
}
