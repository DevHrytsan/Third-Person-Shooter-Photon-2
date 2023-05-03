using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Resolution[] resolutions;

   public SettingsValue settingValue;
    [Header("Panel")]
    public GameObject settingsPanel;

    [Header("Settings")]
    public TMP_Dropdown graphicsDropdown;
    public TMP_Dropdown resolutionDropdown;
    public UnityEngine.UI.Slider volumeSlider;
    public UnityEngine.UI.Toggle fullScreenToggle;


    private void OnEnable()
    {
        settingValue = new SettingsValue();

        Load();
    }
    void Start()
    {
  
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        settingValue.ResolutionIndex = resolutionIndex; 
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Master", volume);
        settingValue.Volume = volume;
    }
    public void SetQuality(int qualityIndex)
    {
       QualitySettings.SetQualityLevel(qualityIndex);
        settingValue.QualityIndex = qualityIndex;
    }
    public void SetFullscreen(bool isFullscreen)
    {
        settingValue.IsFullScreen = Screen.fullScreen = isFullscreen;
    }

    public void Save()
    {
        string jsonData = JsonUtility.ToJson(settingValue, true);
        File.WriteAllText(Application.dataPath +  "/gamesettings.json",jsonData);

    }
    public void ActivePanel(bool status)
    {
        settingsPanel.SetActive(status);
    }
    public void Load()
    {
        if (!Application.isEditor)
        {


            if (File.Exists(Application.dataPath + "/gamesettings.json"))
            {
                settingValue = JsonUtility.FromJson<SettingsValue>(File.ReadAllText(Application.dataPath + "/gamesettings.json"));
                volumeSlider.value = settingValue.Volume;
                graphicsDropdown.value = settingValue.QualityIndex;
                fullScreenToggle.isOn = settingValue.IsFullScreen;
                resolutionDropdown.value = settingValue.ResolutionIndex;
                resolutionDropdown.RefreshShownValue();
            }
            else
            {
                Save();
            }          
        }
        else
        {
            Debug.LogWarning("It`s editor");
        }
    }
    public void Exit()
    {
        if (!Application.isEditor)
        {
            Application.Quit();
        }
        else
        {
            Debug.LogWarning("Exit");
        }

    }
}
