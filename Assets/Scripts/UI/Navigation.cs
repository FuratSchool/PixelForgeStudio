using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Navigation : MonoBehaviour
{
    [SerializeField] private GameObject fistMainMenu;
    [SerializeField] private GameObject firstOptionsMenu;
    [SerializeField] private GameObject BackOptionsMenu;
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(fistMainMenu);
    }

    public void OnOptionsEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOptionsMenu);
    }
    
    public void OnOptionsDisable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(BackOptionsMenu);
    }
}
