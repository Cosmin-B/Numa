using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_SaveLoadManager : MonoBehaviour
{

    private static Game_SaveLoadManager _instance;

    private List<SaveableObject> _saveableObjects;

    // Use this for initialization
    void Awake()
    {
        _saveableObjects = new List<SaveableObject>();

        if (_instance != null)
        {
            Destroy(_instance);
            _instance = this;
        }
        else if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public static Game_SaveLoadManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public List<SaveableObject> GetSaveableObjects
    {
        get
        {
            return _saveableObjects;
        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex.ToString(), _saveableObjects.Count);

        for (int i = 0; i < _saveableObjects.Count; i++)
            _saveableObjects[i].Save(i);
    }

    public void Load()
    {
        foreach (SaveableObject obj in _saveableObjects)
            if (obj != null)
                Destroy(obj.gameObject);

        _saveableObjects.Clear();

        int objectCount = PlayerPrefs.GetInt(SceneManager.GetActiveScene().buildIndex.ToString());

        for(int i = 0; i < objectCount; i++)
        {
            string[] values = PlayerPrefs.GetString(SceneManager.GetActiveScene().buildIndex + "-" + i.ToString()).Split('_');

            GameObject prefab = null;

            switch(values[(int)ReadSaveDataPosition.DATA_OBJECTTYPE])
            {
                case "TYPEPLAYER":
                    prefab = Resources.Load("Prefabs/Player", typeof(GameObject)) as GameObject;
                    break;
                case "TYPEENEMY":
                    break;
                case "TYPEPICKUP":
                    break;
            }

            if(prefab != null)
            {
                GameObject gobj = Instantiate(prefab);
                gobj.GetComponent<SaveableObject>().Load(values);
            }
        }
    }

    public Vector3 StringToVector(string value)
    {
        value = value.Trim(new char[] {'(' , ')' });

        value = value.Replace(" ", "");

        string[] pos = value.Split(',');

        return new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
    }

    public Quaternion StringToQuaternion (string value)
    {
        value = value.Trim(new char[] { '(', ')' });

        value = value.Replace(" ", "");

        string[] pos = value.Split(',');

        return new Quaternion(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]), float.Parse(pos[3]));
    }
}


