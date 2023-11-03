using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Samples.RebindUI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static bool spawned = false;
    
    public InputActionAsset act;
    public AudioMixer audioMixer;
    private string ActiveSceneName;
    public SettingsData Settings { get; set; }
    public RebindActionUI rebindActionUI;
    private void Awake()
    {
        if(spawned == false)
        {
            spawned = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject); 
        }
    }

    private void Start()
    {
        loadSettings();
    }

    public void LoadScene(string sceneName)
    {
        ActiveSceneName = sceneName;
        SceneManager.LoadScene(sceneName);
    }
    
    private void loadSettings()
    {
        Settings = LoadSaveSettings.LoadData();
        audioMixer.SetFloat("MasterVolume", Settings.masterVolume);
        audioMixer.SetFloat("EffectsVolume", Settings.effectsVolume);
        audioMixer.SetFloat("BackgroundVolume", Settings.backgroundVolume);
        Screen.SetResolution(Settings.resolutionWidth, Settings.resolutionHeight, Settings.fullscreen);
        if (!string.IsNullOrEmpty(Settings.rebinds))
            act.LoadBindingOverridesFromJson(Settings.rebinds);
    }
}
