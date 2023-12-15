using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private string _tooltipHeader;
    [SerializeField] private string _tooltipText;
    private GameObject _tooltipObject;
    private bool PrefabActive;
    private bool PrefabSpawned;
    public void OnPointerEnter(PointerEventData eventData)
    {
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
        if (PrefabActive)
        {
            PrefabSpawned = true;
            _tooltipObject = Instantiate(Resources.Load("Prefabs/Tooltip") as GameObject);
            _tooltipObject.transform.SetParent(transform.parent.parent);
            var container = _tooltipObject.transform.GetChild(0).GetChild(0);
            container.transform.GetChild(0).GetComponent<TMP_Text>().text = _tooltipHeader;
            container.transform.GetChild(1).GetComponent<TMP_Text>().text = _tooltipText;
        }
        if(PrefabSpawned && !PrefabActive)
            Destroy(_tooltipObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(_tooltipObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(FindObjectOfType<PlayerInput>().currentControlScheme.Equals("Controller"))
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton16))
            {
                PrefabActive = true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton17))
            {
                PrefabActive = false;
            }
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        PrefabActive = false;
    }
}
