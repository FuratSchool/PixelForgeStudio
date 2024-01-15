using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class NewSettingsMenu : MonoBehaviour
{
    
    [SerializeField] private GameObject _settingsMenu;

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
    
    private Resolution[] _resolutions;
    public GameObject _currentSelecetedhorizontalSelector { get; set; }
    
    private bool InputDisable = false;
    private int activeMenu;
    // Start is called before the first frame update
    void Start()
    {
        activeMenu = startMenu;
        updateMenu(activeMenu);
        InitResolutions();
        
        GeneralButton.GetComponent<Button>().onClick.AddListener(GeneralMenuActive);
        ControlsButton.GetComponent<Button>().onClick.AddListener(ControlsMenuActive);
        AudioButton.GetComponent<Button>().onClick.AddListener(AudioMenuActive);
    }

    // Update is called once per frame

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
        if(!InputDisable)
            _settingsMenu.SetActive(false);
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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(ControlsSlider);
        _currentSelecetedhorizontalSelector = ControlsSlider.transform.parent.gameObject;
        
    }
    void AudioMenuActive()
    {
        GeneralMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        AudioMenu.SetActive(true);
        OptionsButtonHighLightDisable(GeneralButton);
        OptionsButtonHighLightDisable(ControlsButton);
        OptionsButtonHighLightEnable(AudioButton);
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
}
