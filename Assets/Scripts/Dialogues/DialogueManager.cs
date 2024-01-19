using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public GameObject dialogueCanvas;
    private readonly List<DialogueTrigger> dialogueTriggers = new(); // declare a list of DialogueTrigger scripts
    private bool dialogueActive = false;
    private string currentSentence = "";
    private DialogueTrigger dialogueTrigger;
    private bool isTyping;
    private Queue<string> sentences; // FIFO data structure
    private PlayerController PC;
    private bool isdelayTrigger;
    
    private int currentDialogueIndex = 0; 
    private Dialogue dialogue;
    private void Start()
    {
        sentences = new Queue<string>();
        dialogueCanvas.SetActive(false); // hides the canvas when not in dialogue
        var npcGameObjects = GameObject.FindGameObjectsWithTag("NPC");
        foreach (var npcGameObject in npcGameObjects)
        {
            var dialogueTrigger = npcGameObject.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null) dialogueTriggers.Add(dialogueTrigger);
        }
    }

    private void Update()
    {
        if (dialogueActive && !isdelayTrigger)
        {
            if (FindObjectOfType<PlayerController>().InteractPressed)
            {
                
                if (isTyping)
                {
                    GetComponent<AudioSource>().Stop();
                    StopAllCoroutines();
                    StartCoroutine(Wait(0.3f));
                    dialogueText.text = currentSentence;
                    isTyping = false;
                }
                else
                {
                    DisplayNextSentence();
                    if(currentDialogueIndex < dialogue.name.Length -1)
                        currentDialogueIndex++;
                    nameText.text = dialogue.name[currentDialogueIndex];
                }
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogueIndex = 0;
        dialogueCanvas.SetActive(true); // show the canvas when dialogue starts
        dialogueActive = true;
        nameText.text = dialogue.name[currentDialogueIndex];
        sentences.Clear();  
        foreach (var sentence in dialogue.sentences) sentences.Enqueue(sentence);
        DisplayNextSentence();
        this.dialogue = dialogue;
    }

    public void DisplayNextSentence()
    {
        GetComponent<AudioSource>().Play();
        if (sentences.Count == 0)
        {
            EndDialogue();
            dialogueActive = false;
            return;
        }

        currentSentence = sentences.Dequeue();
        dialogueText.text = "";
        start(currentSentence);
    }
    
    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        foreach (var letter in sentence)
        {
            dialogueText.text += letter;
            yield return null;
        }
        GetComponent<AudioSource>().Stop();
        isTyping = false;
    }
    
    private IEnumerator Wait(float waitTime)
    {
        isdelayTrigger = true;
        yield return new WaitForSeconds(waitTime);
        isdelayTrigger = false;
    }

    private void start(string sentence)
    {
        StartCoroutine( Wait(0.3f));
        StartCoroutine( TypeSentence(sentence));
    }

    public void EndDialogue()
    {
        GetComponent<AudioSource>().Stop();
        dialogueCanvas.SetActive(false); // hide the canvas when dialogue ends
        FindObjectOfType<PlayerController>().DialogueActive = false;
    }
}