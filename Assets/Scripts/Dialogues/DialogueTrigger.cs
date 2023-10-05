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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            _inTriggeredZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            _inTriggeredZone = false;
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
        }
    }

    public void EndDialogue()
    {
        _dialogueActive = false;
    }
}