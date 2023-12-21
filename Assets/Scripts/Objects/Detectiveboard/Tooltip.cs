using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler 
{
    [SerializeField] private string _tooltipHeader;
    [SerializeField] private string _tooltipText;
    private Button ObjectButton;
    private GameObject _tooltipObject;
    private bool PrefabActive;
    private bool PrefabSpawned;
    private bool _Selected;

    public void Start()
    {
        ObjectButton = GetComponent<Button>();
        ObjectButton.onClick.AddListener(OnClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(PrefabSpawned) return;
        PrefabSpawned = true;
        PrefabActive = true;
        Vector3 mousePos = Input.mousePosition;
        _tooltipObject = Instantiate(Resources.Load("Prefabs/Tooltip") as GameObject);
        _tooltipObject.transform.SetParent(transform.parent.parent);
        var container = _tooltipObject.transform.GetChild(0).GetChild(0);
        container.transform.GetChild(0).GetComponent<TMP_Text>().text = _tooltipHeader;
        container.transform.GetChild(1).GetComponent<TMP_Text>().text = _tooltipText;
        _tooltipObject.transform.position = mousePos;
        if (mousePos.x > Screen.width - _tooltipObject.transform.GetChild(0).GetComponent<RectTransform>().rect.width)
        {
            _tooltipObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(
                Screen.width - (int)mousePos.x - 50,
                _tooltipObject.transform.GetChild(0).GetComponent<RectTransform>().rect.height);
            _tooltipObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                Screen.width - (int)mousePos.x - 50,
                _tooltipObject.transform.GetComponent<RectTransform>().rect.height);
        }
        _tooltipObject.transform.position = mousePos;
    }

    public void Update()
    {
        if(_Selected){
            // if(FindObjectOfType<PlayerInput>().currentControlScheme.Equals("Controller"))
            // {
                if (PrefabActive && !PrefabSpawned)
                {
                    PrefabSpawned = true;
                    _tooltipObject = Instantiate(Resources.Load("Prefabs/Tooltip") as GameObject);
                    _tooltipObject.transform.SetParent(transform.parent.parent);
                    var container = _tooltipObject.transform.GetChild(0).GetChild(0);
                    container.transform.GetChild(0).GetComponent<TMP_Text>().text = _tooltipHeader;
                    container.transform.GetChild(1).GetComponent<TMP_Text>().text = _tooltipText;
                    _tooltipObject.transform.position = transform.position + new Vector3(250, -125, 0);
                    
                    if (_tooltipObject.transform.position.x > Screen.width - _tooltipObject.transform.GetChild(0).GetComponent<RectTransform>().rect.width)
                    {
                        _tooltipObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(
                            Screen.width - (int)_tooltipObject.transform.position.x - 50,
                            _tooltipObject.transform.GetChild(0).GetComponent<RectTransform>().rect.height);
                        _tooltipObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                            Screen.width - (int)_tooltipObject.transform.position.x - 50,
                            _tooltipObject.transform.GetComponent<RectTransform>().rect.height);
                    }
                }
            // }
        }
        if (!PrefabActive && PrefabSpawned)
        {
            PrefabSpawned = false;
            Destroy(_tooltipObject);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        PrefabActive = false;
        PrefabSpawned = false;
        Destroy(_tooltipObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        _Selected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _Selected = false;
        PrefabActive = false;
    }
    
    public void OnClick()
    {
        PrefabActive = !PrefabActive;
    }
}
