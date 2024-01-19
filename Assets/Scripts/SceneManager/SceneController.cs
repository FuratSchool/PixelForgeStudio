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
    
    public bool FromFirstLevel = false;
    public InputActionAsset act;
    public AudioMixer audioMixer;
    private string ActiveSceneName;
    public SettingsData Settings;
    private GameObject LoadingScreen;
    private bool loadingDone = false;
    private AsyncOperation asyncLoad;
    [SerializeField] private GameObject LoadingScreenPrefab;
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
    public double TimePlayed
    {
        get;
        set;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.F12))
        {
            this.LoadScene("MainMenu");
        }
        if (loadingDone)
        {
            StartCoroutine(LoadTime(3f));
        }
    }

    private void Start()
    {
        LoadingScreen = FindObjectOfType<Navigation>().LoadingScreen;
        loadSettings();
    }
    
    public void LoadSceneAsync(string sceneName)
    {
        if (LoadingScreen == null)
        {
            if(FindObjectOfType<Navigation>() != null)
                LoadingScreen = FindObjectOfType<Navigation>().LoadingScreen;
        }

        if (LoadingScreen == null)
        {
            var obj = Instantiate(LoadingScreenPrefab);
            obj.transform.SetParent(transform.GetChild(0));
            LoadingScreen = obj;
            
        }
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }
    
    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            yield return null;
            loadingDone= true;
        }
    }

    public void DestroyLoading()
    {
        if(LoadingScreen != null)
            Destroy(LoadingScreen);
    }
    
    private IEnumerator LoadTime(float sec)
    {
        yield return new WaitForSeconds(sec);
        asyncLoad.allowSceneActivation = true;
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
    
    public void SaveSettings(SettingsData settingsData)
    {
        LoadSaveSettings.SaveData(settingsData);
    }
}
