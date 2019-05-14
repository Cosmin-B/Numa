using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Inventory : MonoBehaviour
{
    public int batteryCounter = 0;
    public int fuelCounter = 0;

    private GameObject _batteryObject;
    private GameObject _fuelObject;

    private GameObject _batteryObjectMenu;
    private GameObject _fuelObjectMenu;

    private GameObject _laserGunObject;
    private GameObject _wheelObject;
    private GameObject _springsObject;


    private GameObject _laserGunObjectMenu;
    private GameObject _wheelObjectMenu;
    private GameObject _springsObjectMenu;

    public GameObject laserGunOnPlayer;

    private Image _laserGunSprite;
    private Image _wheelSprite;
    private Image _springsSprite;

    private Image _laserGunSpriteMenu;
    private Image _wheelSpriteMenu;
    private Image _springsSpriteMenu;

    private Text _batteryText;
    private Text _fuelText;

    private Text _batteryTextMenu;
    private Text _fuelTextMenu;

    public bool laserGunCollected = false;
    public bool wheelsCollected = false;
    public bool springsCollected = false;
    public bool batteryCollected = false;
    public bool fuelCollected = false;

    private List<string> _itemsInInventory;

    private Game_SoundManager _soundManager = null;

    [HideInInspector] /// these fields are used to load/save objects don't change
    public bool hasWheels = false;
    [HideInInspector] /// these fields are used to load/save objects don't change
    public bool hasLaserGun = false;
    [HideInInspector] /// these fields are used to load/save objects don't change
    public bool hasSprings = false;

    void Awake()
    {

        _batteryObjectMenu = GameObject.FindWithTag("BatteryTextMenu");
        _fuelObjectMenu = GameObject.FindWithTag("FuelTextMenu");

        _laserGunObjectMenu = GameObject.Find("LaserGunImageMenu");
        _wheelObjectMenu = GameObject.Find("WheelImageMenu");
        _springsObjectMenu = GameObject.Find("SpringsImageMenu");

        Debug.Assert(_laserGunObjectMenu != null);
        Debug.Assert(_wheelObjectMenu != null);
        Debug.Assert(_springsObjectMenu != null);
    }

    void Start()
    {
        if (_itemsInInventory == null)
            _itemsInInventory = new List<string>();

        _itemsInInventory.Add(GameManager_References.legsTag);

        _batteryObject = GameObject.FindWithTag("BatteryText");
        _fuelObject = GameObject.FindWithTag("FuelText");

        _batteryText = _batteryObject.GetComponent<Text>();
        _fuelText = _fuelObject.GetComponent<Text>();

        _batteryText = _batteryObject.GetComponent<Text>();
        _fuelText = _fuelObject.GetComponent<Text>();

        _batteryTextMenu = _batteryObjectMenu.GetComponent<Text>();
        _fuelTextMenu = _fuelObjectMenu.GetComponent<Text>();

        Debug.Assert(laserGunOnPlayer != null);
        laserGunOnPlayer.SetActive(false);

        _laserGunObject = GameObject.Find("LaserGunImage");
        _wheelObject = GameObject.Find("WheelImage");
        _springsObject = GameObject.Find("SpringsImage");

        _laserGunSprite = _laserGunObject.GetComponent<Image>();
        _wheelSprite = _wheelObject.GetComponent<Image>();
        _springsSprite = _springsObject.GetComponent<Image>();

        _laserGunSpriteMenu = _laserGunObjectMenu.GetComponent<Image>();
        _wheelSpriteMenu = _wheelObjectMenu.GetComponent<Image>();
        _springsSpriteMenu = _springsObjectMenu.GetComponent<Image>();

        _laserGunSprite.color = new Color(0f, 0f, 0f);
        _wheelSprite.color = new Color(0f, 0f, 0f);
        _springsSprite.color = new Color(0f, 0f, 0f);

        _laserGunSpriteMenu.color = new Color(0f, 0f, 0f);
        _wheelSpriteMenu.color = new Color(0f, 0f, 0f);
        _springsSpriteMenu.color = new Color(0f, 0f, 0f);

        LoadInventory();

        _soundManager = Game_Manager.Instance.GetSoundManager();

        Debug.Assert(_soundManager != null);
        Debug.Assert(_laserGunObject != null);
        Debug.Assert(_wheelObject != null);
        Debug.Assert(_springsObject != null);
    }

    public void AddItemToInventory(GameObject item)
    {
        if (item.tag == GameManager_References.batteryTag)
        {
           
            batteryCounter++;
            _itemsInInventory.Add(item.tag);
            Destroy(item);
            UpdateInventory();
            batteryCollected = true;
            _soundManager.PlaySoundForPlayer(Player_SoundHolder.collectUsable);
        }
        else if (item.tag == GameManager_References.fuelTag)
        {
            fuelCounter++;
            _itemsInInventory.Add(item.tag);
            Destroy(item);
            UpdateInventory();
            fuelCollected = true;
            _soundManager.PlaySoundForPlayer(Player_SoundHolder.collectUsable);
        }
        else if (item.tag == GameManager_References.gunTag)
        {
            _itemsInInventory.Add(item.tag);
            laserGunOnPlayer.SetActive(true);
            Destroy(item);
            _laserGunSprite.color = new Color(255f, 255f, 255f); 
            _laserGunSpriteMenu.color = new Color(255f, 255f, 255f);
            laserGunCollected = true;
            _soundManager.PlaySoundForPlayer(Player_SoundHolder.collectItem);
        }
        else if (item.tag == GameManager_References.wheelsTag)
        {
            _itemsInInventory.Add(item.tag);
            Destroy(item);
            _wheelSprite.color = new Color(255f, 255f, 255f);
            _wheelSpriteMenu.color = new Color(255f, 255f, 255f);
            wheelsCollected = true;
            _soundManager.PlaySoundForPlayer(Player_SoundHolder.collectItem);
        }
        else if (item.tag == GameManager_References.springsTag)
        {
            GetComponent<Player_Controller>().enableJump = true;
            _itemsInInventory.Add(item.tag);
            Destroy(item);
            _springsSprite.color = new Color(255f, 255f, 255f);
            _springsSpriteMenu.color = new Color(255f, 255f, 255f);
            springsCollected = true;
            _soundManager.PlaySoundForPlayer(Player_SoundHolder.collectItem);
        }
    }

    private void UpdateInventory()
    {
        _batteryText.text = " " + batteryCounter;
        _fuelText.text = " " + fuelCounter;
        _batteryTextMenu.text = " " + batteryCounter;
        _fuelTextMenu.text = " " + fuelCounter;
    }

    public bool HasItemWithTagInInventory(string itemTag)
    {
        bool found = false;

        foreach (string tag in _itemsInInventory)
        {
            if (tag == itemTag)
            {
                found = true;
                break;
            }
        }

        return found;
    }

    public void LoadInventory()
    {
        /// add items in inventory based on how many are after load save progress
        for (int i = 0; i < batteryCounter; i++)
            _itemsInInventory.Add(GameManager_References.batteryTag);

        if (hasWheels)
        {
            _itemsInInventory.Add(GameManager_References.wheelsTag);
            _wheelSprite.color = new Color(255f, 255f, 255f);
            _wheelSpriteMenu.color = new Color(255f, 255f, 255f);
        }

        if (hasLaserGun)
        {
            laserGunOnPlayer.SetActive(true);
            _laserGunSprite.color = new Color(255f, 255f, 255f);
            _laserGunSpriteMenu.color = new Color(255f, 255f, 255f);
            _itemsInInventory.Add(GameManager_References.gunTag);
        }

        if(hasSprings)
        {
            GetComponent<Player_Controller>().enableJump = true;
            _itemsInInventory.Add(GameManager_References.springsTag);
            _springsSprite.color = new Color(255f, 255f, 255f);
            _springsSpriteMenu.color = new Color(255f, 255f, 255f);
        }

        UpdateInventory();
    }

    public bool RemovePowerupFromInventory(bool battery)
    {
        if (battery)
        {
            List<string> batteriesList = _itemsInInventory.FindAll(x => x.Contains(GameManager_References.batteryTag));

            if (batteriesList.Count > 0)
            {
                _itemsInInventory.Remove(batteriesList[0]);
                batteryCounter--;
                UpdateInventory();
                return true;
            }
        }
        else
        {
            List<string> fuelList = _itemsInInventory.FindAll(x => x.Contains(GameManager_References.fuelTag));

            if(fuelList.Count > 0)
            {
                _itemsInInventory.Remove(fuelList[0]);
                fuelCounter--;
                UpdateInventory();
                return true;
            }
        }
        return false;
    }
}
