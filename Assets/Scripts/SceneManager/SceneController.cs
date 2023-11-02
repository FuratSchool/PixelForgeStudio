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
    public SettingsData settings;
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
        //UpdateObjects();
    }
    
    private void UpdateObjects()
    {
        GameObject[] objects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objects)
        {
            obj.SendMessage("OnSceneLoaded",SendMessageOptions.DontRequireReceiver);
        }
    }

    private void loadSettings()
    {
        settings = LoadSaveSettings.LoadData();
        audioMixer.SetFloat("MasterVolume", settings.masterVolume);
        audioMixer.SetFloat("EffectsVolume", settings.effectsVolume);
        audioMixer.SetFloat("BackgroundVolume", settings.backgroundVolume);
        Screen.SetResolution(settings.resolutionWidth, settings.resolutionHeight, settings.fullscreen);
        if (!string.IsNullOrEmpty(settings.rebinds))
            act.LoadBindingOverridesFromJson(settings.rebinds);
    }
    
}
