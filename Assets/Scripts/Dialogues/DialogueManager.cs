using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public GameObject dialogueCanvas;
    Queue<string> sentences; // FIFO data structure
    bool isTyping = false;
    string currentSentence = "";
    List<DialogueTrigger> dialogueTriggers = new List<DialogueTrigger>(); // declare a list of DialogueTrigger scripts
    DialogueTrigger dialogueTrigger;
    private bool isDialogueActive = false;


    void Start(){
        sentences = new Queue<string>();
        dialogueCanvas.SetActive(false); // hides the canvas when not in dialogue
        GameObject[] npcGameObjects = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject npcGameObject in npcGameObjects)
        {
            DialogueTrigger dialogueTrigger = npcGameObject.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null)
            {
                dialogueTriggers.Add(dialogueTrigger);
            }
        }
    }

    public void StartDialogue(Dialogue dialogue) 
    {
        FindObjectOfType<PlayerMovement>().SetCanMove(false);
        FindObjectOfType<HoldJumping>().SetCanJump(false);
        dialogueCanvas.SetActive(true); // show the canvas when dialogue starts

        nameText.text = dialogue.name;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }
        currentSentence = sentences.Dequeue();
        dialogueText.text = "";
        StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        isTyping = false;
    }

    public void EndDialogue() {
        FindObjectOfType<PlayerMovement>().SetCanMove(true);
        FindObjectOfType<HoldJumping>().SetCanJump(true);
        dialogueCanvas.SetActive(false); // hide the canvas when dialogue ends
        
        foreach (DialogueTrigger dialogueTrigger in dialogueTriggers)
        {
            dialogueTrigger.EndDialogue();
        }
    }

        void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentSentence;
                isTyping = false;
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }
}