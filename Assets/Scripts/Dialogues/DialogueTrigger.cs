using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool canTalkAgain = false;
    private bool inTriggeredZone = false;
    private bool dialogueActive = false;
    private bool dialogueTriggered = false;

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Player"){
            inTriggeredZone = true;
        }
    }
    private void OnTriggerExit(Collider other){
        if (other.gameObject.tag == "Player"){
            inTriggeredZone = false;
        }
    }
    private void Update()
    {
        if (inTriggeredZone && Input.GetKeyDown(KeyCode.F) && (!dialogueActive || canTalkAgain) && !dialogueTriggered){
            dialogueActive = true;
            dialogueTriggered = true;
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        }
    }

    public void EndDialogue()
    {
        dialogueActive = false;
        dialogueTriggered = false;
    }
}