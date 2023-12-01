using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Samples.RebindUI;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

public class SettingsMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    Resolution[] _resolutions;
    private SettingsData settings;
    public InputActionAsset actions;
    public bool InGameScene {get; set;} =false;
    [SerializeField] private Color TextColor;
    [SerializeField] private GameObject ControllerMap;
    [SerializeField] private GameObject KeyboardMap;
    private void Awake()
    {
        InitResolutions();
    }

    private void OnDisable()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        settings.rebinds = rebinds;
        FindObjectOfType<SceneController>().Settings.rebinds = rebinds;
        LoadSaveSettings.SaveData(settings);
    }

    private void Start()
    {
        settings = FindObjectOfType<SceneController>().Settings;
        actions = FindObjectOfType<SceneController>().act;
    }
    
    public AudioMixer audioMixer;
    // Start is called before the first frame update
    public void SetMasterVolume(float volume)
    {
        settings.masterVolume = volume;
        audioMixer.SetFloat("MasterVolume", settings.masterVolume);
    }
    public void SetEffectsVolume(float volume)
    {
        settings.effectsVolume = volume;
        audioMixer.SetFloat("EffectsVolume", settings.effectsVolume);
    }
    public void SetBackgroundVolume(float volume)
    {
        settings.backgroundVolume = volume;
        audioMixer.SetFloat("BackgroundVolume", settings.backgroundVolume);
    }
    
    public void SetFullscreen(bool isFullscreen)
    {
        settings.fullscreen = isFullscreen;
        Screen.fullScreen = settings.fullscreen;
    }
    
    public void SetInvertedY(bool isInverted)
    {
        settings.invertedY = isInverted;
        if (InGameScene)
        {
            FindObjectOfType<CameraController>().UpdateCameraSettings(settings);
        }
        
    }
    
    public void SetInvertedX(bool isInverted)
    {
        settings.invertedX = isInverted;
        if (InGameScene)
        {
            FindObjectOfType<CameraController>().UpdateCameraSettings(settings);
        }
    }
    
    public void SetSensitivityY(float sensitivity)
    {
        settings.sensitivityY = sensitivity;
        if (InGameScene)
        {
            FindObjectOfType<CameraController>().UpdateCameraSettings(settings);
        }
    }
    
    public void SetSensitivityX(float sensitivity)
    {
        settings.sensitivityX = sensitivity;
        if (InGameScene)
        {
            FindObjectOfType<CameraController>().UpdateCameraSettings(settings);
        }
    }

    private void InitResolutions()
    {
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        
        int currentResolutionIndex = 0;
        List<string> options = new List<string>();

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width+"x"+_resolutions[i].height + "@" + _resolutions[i].refreshRate + "Hz";
            options.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        //resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        settings.resolutionHeight = resolution.height;
        settings.resolutionWidth = resolution.width;
        Screen.SetResolution(settings.resolutionWidth,settings.resolutionHeight,Screen.fullScreen);
    }
    public void UpdateAudio()
    {
        GameObject.Find("MasterVolume").transform.GetChild(0).GetComponent<Slider>().value = settings.masterVolume;
        GameObject.Find("BackgroundVolume").transform.GetChild(0).GetComponent<Slider>().value = settings.backgroundVolume;
        GameObject.Find("EffectsVolume").transform.GetChild(0).GetComponent<Slider>().value = settings.effectsVolume;
    }

    public void UpdateVideo()
    {
        int index = Array.FindIndex(_resolutions, res => res.width == settings.resolutionWidth && res.height == settings.resolutionHeight);
        GameObject.Find("Resolution").transform.GetChild(0).GetComponent<TMP_Dropdown>().value = index;
        GameObject.Find("Fullscreen").transform.GetChild(0).GetComponent<Toggle>().isOn = settings.fullscreen;
    }

    public void UpdateControls()
    {
        GameObject.Find("InvertedCamera").transform.GetChild(2).GetComponent<Toggle>().isOn = settings.invertedY;
        GameObject.Find("InvertedCamera").transform.GetChild(0).GetComponent<Toggle>().isOn = settings.invertedX;
        GameObject.Find("CameraSpeedY").transform.GetChild(0).GetComponent<Slider>().value = settings.sensitivityY;
        GameObject.Find("CameraSpeedX").transform.GetChild(0).GetComponent<Slider>().value = settings.sensitivityX;
    }
    

    public void SetColorBlack(TMP_Text text)
    {
        text.color = Color.black;
    }
    public void SetColorOptionsMenu(TMP_Text text)
    {
        text.color = TextColor;
    }
    
    public void SetMap(int MapIndex)
    {
        
        if (MapIndex == 1)
        {
            ControllerMap.SetActive(true);
            KeyboardMap.SetActive(false);
        }
        else
        {
            ControllerMap.SetActive(false);
            KeyboardMap.SetActive(true);
        }
    }
    
    public void OptionsButtonHighLightEnable(GameObject Object)
    {
        if (Object.transform.parent.GetComponent<UnityEngine.UI.Button>().interactable)
            Object.GetComponent<TMP_Text>().fontStyle |= FontStyles.Underline;
    }
    public void OptionsButtonHighLightDisable(GameObject Object)
    {
        
        Object.GetComponent<TMP_Text>().fontStyle &= ~FontStyles.Underline;
    }
}
