using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Options_Manager : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Dropdown textureQualityDropdown;
    public Dropdown antialiasingDropdown;
    public Dropdown vSyncDropdown;
    public Slider musicVolumeSlider;
    public Resolution[] resolutionArray;
    public Button applyButton;

    public GameSettings gameSettings;

    public AudioMixer audioMixer;

    private void OnEnable()
    {
        gameSettings = new GameSettings();

        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullScreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        antialiasingDropdown.onValueChanged.AddListener(delegate { OnantialiasingChange(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { OnvSyncChange(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
        applyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });

        resolutionArray = Screen.resolutions;
        foreach(Resolution resolution in resolutionArray)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }
    }

    public void Start()
    {
        LoadSettings();
    }

    public void OnFullScreenToggle()
    {
       gameSettings.fullscreen =  Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutionArray[resolutionDropdown.value].width, resolutionArray[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionindex = resolutionDropdown.value;
    }

    public void OnTextureQualityChange()
    {
       QualitySettings.masterTextureLimit =  gameSettings.textureQuality = textureQualityDropdown.value;

    }

    public void OnantialiasingChange()
    {
        QualitySettings.antiAliasing = gameSettings.antialiasing = (int)Mathf.Pow(2f, antialiasingDropdown.value);
    }

    public void OnvSyncChange()
    {
        QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
    }

    public void OnMusicVolumeChange()
    {
        audioMixer.SetFloat("masterVolume", musicVolumeSlider.value);
        gameSettings.musicVolume = musicVolumeSlider.value;
    }

    public void OnApplyButtonClick()
    {
        SaveSettings();
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }

    public void LoadSettings()
    {
        if (!File.Exists(Application.persistentDataPath + "/gamesettings.json"))
            return;

        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));

        musicVolumeSlider.value = gameSettings.musicVolume;
        audioMixer.SetFloat("masterVolume", gameSettings.musicVolume);
        antialiasingDropdown.value = gameSettings.antialiasing;
        vSyncDropdown.value = gameSettings.vSync;
        textureQualityDropdown.value = gameSettings.textureQuality;
        resolutionDropdown.value = gameSettings.resolutionindex;
        fullscreenToggle.isOn = gameSettings.fullscreen;
        resolutionDropdown.RefreshShownValue();

        Screen.fullScreen = gameSettings.fullscreen;
    }
}
