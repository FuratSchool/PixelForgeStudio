using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool canTalkAgain = true; //allows initial interaction with npc.
    public bool hasBeenTalkedTo = false;
    private UIController _uiController;

    private void Start()
    {
        _uiController = FindObjectOfType<UIController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            if ((canTalkAgain || !hasBeenTalkedTo))
            {
                FindObjectOfType<PlayerController>().NPC = this;
                FindObjectOfType<PlayerController>().InDialogeTriggerZone = true;
                
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            FindObjectOfType<PlayerController>().InDialogeTriggerZone = false;
            _uiController.SetInteractableTextActive(false);
        }
    }
}