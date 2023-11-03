using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool canTalkAgain = true; //allows initial interaction with npc.
    private bool hasBeenTalkedTo = false;
    private bool _inTriggeredZone = false;
    private bool _dialogueActive = false;
    private UIController _uiController;

    private void Start()
    {
        _uiController = FindObjectOfType<UIController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            _inTriggeredZone = true;
            if ((canTalkAgain || !hasBeenTalkedTo))
            {
                _uiController.SetInteractText("Press F to talk");
                _uiController.SetInteractableTextActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            _inTriggeredZone = false;
            _uiController.SetInteractableTextActive(false);
        }
    }
    private void Update()
    {
        if (_inTriggeredZone && Input.GetKeyDown(KeyCode.F) && !_dialogueActive && (canTalkAgain || !hasBeenTalkedTo)){
            _dialogueActive = true;
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            if (!canTalkAgain && !hasBeenTalkedTo)
            {
                hasBeenTalkedTo = true;
            }
            _uiController.SetInteractableTextActive(false);
        }
    }

    public void EndDialogue()
    {
        _dialogueActive = false;
        if (canTalkAgain && _inTriggeredZone) // Show the canvas again for repeatable dialogues.
        {
            _uiController.SetInteractText("Press F to talk");
            _uiController.SetInteractableTextActive(true);
        }
    }
}