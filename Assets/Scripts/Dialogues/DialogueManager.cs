using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    void Start(){
        sentences = new Queue<string>();
        dialogueCanvas.SetActive(false); // hideS the canvas when not in dialogue
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
        Debug.Log("Starting conversation with " + dialogue.name);
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
        dialogueCanvas.SetActive(true); // show the canvas when dialogue starts
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
        dialogueCanvas.SetActive(false); // hide the canvas when dialogue ends
        Debug.Log("End of conversation");
        foreach (DialogueTrigger dialogueTrigger in dialogueTriggers)
        {
            dialogueTrigger.EndDialogue();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
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