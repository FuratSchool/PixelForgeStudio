using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class NewSettingsMenu : MonoBehaviour
{
    
    [SerializeField] private GameObject _settingsMenu;
    
    [SerializeField] private InputActionAsset actions;
    [SerializeField] private GameObject ControlsButton;
    [SerializeField] private GameObject GeneralButton;
    [SerializeField] private GameObject AudioButton;
    
    [SerializeField] private GameObject ControlsMenu;
    [SerializeField] private GameObject GeneralMenu;
    [SerializeField] private GameObject AudioMenu;
    [SerializeField] private GameObject ControlsSlider;
    [Range(1,3)]
    [SerializeField] private int startMenu = 2;
    [SerializeField] private HorizontalSelector horizontalSelector;


    [SerializeField] private Slider MasterVolumeSilder;
    [SerializeField] private Slider BackgroundVolumeSlider;
    [SerializeField] private Slider EffectsVolumeSlider;
    [SerializeField] private HorizontalSelector ResolutionDropdown;
    [SerializeField] private HorizontalSelector FullscreenToggle;

    [SerializeField] private HorizontalSelector CameraInvertYToggle;
    [SerializeField] private HorizontalSelector CameraInvertXToggle;
    [SerializeField] private HorizontalSelector CameraSensitivityDropdown;
    [SerializeField] private GameObject backToMenu;
    
    [SerializeField] private SceneController sceneController;
    private AudioMixer audioMixer;
    
    private Resolution[] _resolutions;
    public GameObject _currentSelecetedhorizontalSelector { get; set; }
    private PlayerInput _input;
    public bool InGameScene {get; set;} =false;
    private bool InputDisable = false;
    private int activeMenu;
    
    void Start()
    {
        audioMixer = Resources.Load("Audio/MainMixer", typeof(AudioMixer)) as AudioMixer;
        //this._settings = sceneController.Settings;
        activeMenu = startMenu;
        updateMenu(activeMenu);
        InitResolutions();
        if (InGameScene)
        {
            GetComponent<PlayerInput>().enabled = false;
            _input = GameObject.Find("PlayerObject").GetComponent<PlayerInput>();
        }
        else
        {
            _input = GetComponent<PlayerInput>();
        }
        GeneralButton.GetComponent<Button>().onClick.AddListener(GeneralMenuActive);
        ControlsButton.GetComponent<Button>().onClick.AddListener(ControlsMenuActive);
        AudioButton.GetComponent<Button>().onClick.AddListener(AudioMenuActive);
        InputUser.onChange += UserChangedControls;
        updateSettings();
        
    }
    private void UserChangedControls(InputUser user, InputUserChange change, InputDevice device)
    {
        if ((user.controlScheme.Value.name.Equals("Controller")))
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                if (activeMenu == 1)
                {
                    _currentSelecetedhorizontalSelector = ControlsSlider.transform.parent.gameObject;
                    SelectControlsMenu();
                }
                else if (activeMenu == 2)
                    SelectGeneralMenu();
                else if (activeMenu == 3)
                    SelectAudioMenu();
            }
        }
    }
    private void updateSettings()
    {
        UpdateAudio();
        UpdateControls();
        UpdateVideo();
        actions.LoadBindingOverridesFromJson(sceneController.Settings.rebinds);
    }
    private void OnEnable()
    {
        if(_input != null)
            _input.actions.actionMaps[1].Enable();
        InputDisable = false;
        GeneralMenuActive();
    }
    
    void OnMenuLeft()
    {
        if(InputDisable)
            return;
        activeMenu--;
        if (activeMenu < 1)
            activeMenu = 3;
        updateMenu(activeMenu);
    }
    void OnMenuRight()
    {
        if(InputDisable)
            return;
        activeMenu++;
        if (activeMenu > 3)
            activeMenu = 1;
        updateMenu(activeMenu);
    }
    
    void OnBack(InputValue inputValue)
    {
        if (!InputDisable)
        {
            _input.actions.actionMaps[0].Enable();
            InputDisable = false;
            sceneController.Settings.rebinds = _input.actions.SaveBindingOverridesAsJson();
            Debug.Log(sceneController.Settings.masterVolume);
            
            _settingsMenu.SetActive(false);
            
            activeMenu = startMenu;
            
            var rebinds = actions.SaveBindingOverridesAsJson();
            sceneController.Settings.rebinds = rebinds;
            sceneController.SaveSettings(sceneController.Settings);
            
            backToMenu.SetActive(true);
            var player_input = transform.parent.GetComponent<PlayerInput>();
            if(player_input != null)
                player_input.enabled = true;
        }
    }

    void OnReset()
    {
        if(InputDisable) return;
        sceneController.Settings = new SettingsData("",1920,1080,0,0,0,true,false,false,1);
        updateSettings();
    }
    void OnLeftright(InputValue inputValue)
    {
        if(InputDisable)
            return;
        if(_currentSelecetedhorizontalSelector == null)
            return;
        if (inputValue.Get<float>() > 0)
            _currentSelecetedhorizontalSelector.GetComponent<HorizontalSelector>().OnRightClick();
        else
            _currentSelecetedhorizontalSelector.GetComponent<HorizontalSelector>().OnLeftClick();
    }
    public void updateMenu(int activeMenu)
    {
        switch (activeMenu)
        {
            case 1:
                ControlsMenuActive();
                break;
            case 2:
                
                GeneralMenuActive();
                break;
            case 3:
                AudioMenuActive();
                break;
        }
        
    }
    void GeneralMenuActive()
    {
        GeneralMenu.SetActive(true);
        ControlsMenu.SetActive(false);
        AudioMenu.SetActive(false);
        OptionsButtonHighLightEnable(GeneralButton);
        OptionsButtonHighLightDisable(ControlsButton);
        OptionsButtonHighLightDisable(AudioButton);
        SelectGeneralMenu();
    }

    private void SelectGeneralMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(GeneralMenu.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
    }
    void ControlsMenuActive()
    {
        GeneralMenu.SetActive(false);
        ControlsMenu.SetActive(true);
        AudioMenu.SetActive(false);
        OptionsButtonHighLightDisable(GeneralButton);
        OptionsButtonHighLightEnable(ControlsButton);
        OptionsButtonHighLightDisable(AudioButton);
        SelectControlsMenu();
        _currentSelecetedhorizontalSelector = ControlsSlider.transform.parent.gameObject;
    }
    
    private void SelectControlsMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(ControlsSlider);
        
    }
    void AudioMenuActive()
    {
        GeneralMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        AudioMenu.SetActive(true);
        OptionsButtonHighLightDisable(GeneralButton);
        OptionsButtonHighLightDisable(ControlsButton);
        OptionsButtonHighLightEnable(AudioButton);
        SelectAudioMenu();
    }
    
    private void SelectAudioMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(AudioMenu.transform.GetChild(0).GetChild(0).gameObject);
        
    }
    
    public void OptionsButtonHighLightEnable(GameObject Object)
    {
        if (Object.GetComponent<Button>().interactable)
            Object.GetComponentInChildren<TMP_Text>().fontStyle |= FontStyles.Underline;
    }
    public void OptionsButtonHighLightDisable(GameObject Object)
    {
        
        Object.GetComponentInChildren<TMP_Text>().fontStyle &= ~FontStyles.Underline;
    }
    
    public void DisableInput(bool toggle)
    {
        InputDisable = toggle;
    }
    
    private void InitResolutions()
    {
        _resolutions = Screen.resolutions;
        horizontalSelector.ClearOptions();
        
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
        horizontalSelector.AddOptions(options);
        //resolutionDropdown.value = currentResolutionIndex;
        horizontalSelector.RefreshShownValue();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBack(null);
        }
    }

    public void SetMasterVolume(float volume)
    {
        check_scenecontroller();
        sceneController.Settings.masterVolume = volume;
        Debug.Log(sceneController.Settings.masterVolume);
        if(audioMixer == null)
            audioMixer = Resources.Load("Audio/MainMixer", typeof(AudioMixer)) as AudioMixer;
        audioMixer.SetFloat("MasterVolume", sceneController.Settings.masterVolume);
    }
    public void SetEffectsVolume(float volume)
    {
        check_scenecontroller();
        sceneController.Settings.effectsVolume = volume;
        if(audioMixer == null)
            audioMixer = Resources.Load("Audio/MainMixer", typeof(AudioMixer)) as AudioMixer;
        audioMixer.SetFloat("EffectsVolume", sceneController.Settings.effectsVolume);
    }
    public void SetBackgroundVolume(float volume)
    {
        check_scenecontroller();
        sceneController.Settings.backgroundVolume = volume;
        if(audioMixer == null)
            audioMixer = Resources.Load("Audio/MainMixer", typeof(AudioMixer)) as AudioMixer;
        audioMixer.SetFloat("BackgroundVolume", sceneController.Settings.backgroundVolume);
    }
    
    public void SetFullscreen(int isFullscreen)
    {
        check_scenecontroller();
        sceneController.Settings.fullscreen = Convert.ToBoolean(isFullscreen);
        Screen.fullScreen = sceneController.Settings.fullscreen;
    }
    
    public void SetInvertedX(int isInverted)
    {
        check_scenecontroller();
        sceneController.Settings.invertedX = Convert.ToBoolean(isInverted);
        if (InGameScene)
        {
            FindObjectOfType<CameraController>().UpdateCameraSettings(sceneController.Settings);
        }
    }
    
    public void SetCameraSensitivity(int setting)
    {
        check_scenecontroller();
        sceneController.Settings.invertedX = Convert.ToBoolean(setting);
        if (InGameScene)
        {
            FindObjectOfType<CameraController>().UpdateCameraSettings(sceneController.Settings);
        }
    }
    
    public void SetInvertedY(int isInverted)
    {
        check_scenecontroller();
        sceneController.Settings.invertedY = Convert.ToBoolean(isInverted);
        if (InGameScene)
        {
            FindObjectOfType<CameraController>().UpdateCameraSettings(sceneController.Settings);
        }
        
    }
    
    public void SetResolution(int resolutionIndex)
    {
        check_scenecontroller();
        Resolution resolution = _resolutions[resolutionIndex];
        sceneController.Settings.resolutionHeight = resolution.height;
        sceneController.Settings.resolutionWidth = resolution.width;
        Screen.SetResolution(sceneController.Settings.resolutionWidth,sceneController.Settings.resolutionHeight,Screen.fullScreen);
    }
    
    public void UpdateAudio()
    {
        MasterVolumeSilder.value = sceneController.Settings.masterVolume;
        BackgroundVolumeSlider.value = sceneController.Settings.backgroundVolume;
        EffectsVolumeSlider.value = sceneController.Settings.effectsVolume;
    }

    public void UpdateVideo()
    {
        
        int index = Array.FindIndex(_resolutions, res => res.width == sceneController.Settings.resolutionWidth && res.height == sceneController.Settings.resolutionHeight);
        ResolutionDropdown.Value = index;
        ResolutionDropdown.RefreshShownValue();
        FullscreenToggle.Value = Convert.ToInt32(sceneController.Settings.fullscreen);
        FullscreenToggle.RefreshShownValue();
    }

    public void UpdateControls()
    {
        CameraInvertYToggle.Value = Convert.ToInt32(sceneController.Settings.invertedY);
        CameraInvertXToggle.Value = Convert.ToInt32(sceneController.Settings.invertedX);
        CameraSensitivityDropdown.Value = sceneController.Settings.sensitivity;
        CameraInvertYToggle.RefreshShownValue();
        CameraInvertXToggle.RefreshShownValue();
        CameraSensitivityDropdown.RefreshShownValue();
    }

    public void check_scenecontroller()
    {
        if(sceneController == null)
            sceneController = FindObjectOfType<SceneController>();
    }

}
