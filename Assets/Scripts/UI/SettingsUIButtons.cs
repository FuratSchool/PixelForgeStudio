
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsUIButtons : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Sprite sprite;
    private Image image;
    private HorizontalSelector selector;
    [SerializeField] private bool isKeyRebindButton = false;
    [SerializeField] private bool isFirstButton = false;
    [SerializeField] private bool isHorizontalSelector = false;
    private NewSettingsMenu _newSettingsMenu;
    public void Start()
    {
        if(isKeyRebindButton)
            image = GetComponent<RectTransform>().parent.parent.gameObject.GetComponent<Image>();
        else if (isHorizontalSelector)
        {
            image = GetComponent<RectTransform>().parent.parent.gameObject.GetComponent<Image>();
            selector = GetComponent<RectTransform>().parent.GetComponent<HorizontalSelector>();
            _newSettingsMenu = FindObjectOfType<NewSettingsMenu>();
        }
        else
            image = GetComponent<RectTransform>().parent.gameObject.GetComponent<Image>();
        sprite = Resources.Load("UI/SettingHighlighter", typeof(Sprite)) as Sprite;
        if (isFirstButton)
            SetHighlight(true);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (isHorizontalSelector)
        {
            if (_newSettingsMenu != null)
            {
                _newSettingsMenu._currentSelecetedhorizontalSelector = selector.gameObject;
            }
        }
        SetHighlight(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        
        if(isHorizontalSelector)
            _newSettingsMenu._currentSelecetedhorizontalSelector = null;
        SetHighlight(false);
    }

    public void OnDisable()
    {
        SetHighlight(false);
    }

    private void SetHighlight(bool set)
    {
        if (image == null) return;
        if (set)
        {
            image.sprite = sprite;
            image.color = Color.white;
        }
        else
        {
            image.sprite = null;
            image.color = Color.clear;
        }
    }
}
