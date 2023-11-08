using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindSaveLoad : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private InputActionAsset actions;
    private SettingsData settings;
    private void OnDisable()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        settings.rebinds = rebinds;
        FindObjectOfType<SceneController>().Settings.rebinds = rebinds;
    }
    
    void OnEnable()
    {
        UpdateKeybinds();
    }
    
    public void UpdateKeybinds()
    {
        if (!string.IsNullOrEmpty(FindObjectOfType<SceneController>().Settings.rebinds))
            actions.LoadBindingOverridesFromJson(FindObjectOfType<SceneController>().Settings.rebinds);
    }
}
