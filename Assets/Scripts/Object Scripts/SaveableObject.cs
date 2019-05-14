using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ReadSaveDataPosition : int
{
    DATA_OBJECTTYPE = 0, // object type
    DATA_TRANSFORM_POSITION = 1, // object transform position
    DATA_TRANSFORM_LOCALSCALE = 2, // object transform local scale
    DATA_TRANSFORM_LOCALROTATION = 3, // object transform local rotation
};

public enum GameObjectType { TYPEPLAYER, TYPEENEMY, TYPEPICKUP };

public abstract class SaveableObject : MonoBehaviour {

    protected string saveSpecificData;

    [SerializeField]
    protected GameObjectType objectType;

    // Use this for initialization
    void Start()
    {
        Game_SaveLoadManager.Instance.GetSaveableObjects.Add(this);
    }

    public virtual void Save(int id)
    {
        PlayerPrefs.SetString(SceneManager.GetActiveScene().buildIndex + "-" + id.ToString(), 
            objectType + "_" 
            + transform.position.ToString() + "_"
            + transform.localScale.ToString() + "_"
            + transform.localRotation.ToString() + "_"
            + saveSpecificData);
    }

    public virtual void Load(string[] values)
    {
        transform.position = Game_SaveLoadManager.Instance.StringToVector(values[(int)ReadSaveDataPosition.DATA_TRANSFORM_POSITION]);
        transform.localScale = Game_SaveLoadManager.Instance.StringToVector(values[(int)ReadSaveDataPosition.DATA_TRANSFORM_LOCALSCALE]);
        //transform.localRotation = Game_SaveLoadManager.Instance.StringToQuaternion(values[(int)ReadSaveDataPosition.DATA_TRANSFORM_LOCALROTATION]);
    }

    public void DestroySaveableObject()
    {
        Game_SaveLoadManager.Instance.GetSaveableObjects.Remove(this);
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        DestroySaveableObject();
    }
}
