using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string InteractText { get; set;}
    public bool InteractableTextActive { get; set; }
    private TMP_Text textElement;
    private bool lastState = false;
    // Start is called before the first frame update
    void Start()
    {
        textElement = transform.GetChild(0).GetComponent<TMP_Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InteractableTextActive != lastState)
        {
            lastState = InteractableTextActive;
            if (InteractableTextActive)
            {
                textElement.text = InteractText;
                textElement.gameObject.SetActive(true);
            }
            else
            {
                textElement.gameObject.SetActive(false);
            }
        }
    }
}
