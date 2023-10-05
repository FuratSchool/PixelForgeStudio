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
    public GameObject dialogueTriggerZoneCanvas;

    private void Start()
    {
        dialogueTriggerZoneCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            _inTriggeredZone = true;
            if ((canTalkAgain || !hasBeenTalkedTo))
            {
                dialogueTriggerZoneCanvas.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            _inTriggeredZone = false;
            if (!canTalkAgain) // If the dialogue is not repeatable, hide the canvas when exiting.
            {
                dialogueTriggerZoneCanvas.SetActive(false);
            }
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
            dialogueTriggerZoneCanvas.SetActive(false);
        }
    }

    public void EndDialogue()
    {
        _dialogueActive = false;
        if (canTalkAgain && _inTriggeredZone) // Show the canvas again for repeatable dialogues.
        {
            dialogueTriggerZoneCanvas.SetActive(true);
        }
    }
}