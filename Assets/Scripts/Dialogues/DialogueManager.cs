using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public GameObject dialogueCanvas;
    private readonly List<DialogueTrigger> dialogueTriggers = new(); // declare a list of DialogueTrigger scripts
    private string currentSentence = "";
    private DialogueTrigger dialogueTrigger;
    private bool isDialogueActive = false;
    private bool isTyping;
    private Queue<string> sentences; // FIFO data structure


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

    public void StartDialogue(Dialogue dialogue)
    {
        FindObjectOfType<PlayerController>().CanMove = false;
        FindObjectOfType<PlayerStateMachine>().CurrentState = null;
        FindObjectOfType<PlayerController>().SetCanJump(false);
        dialogueCanvas.SetActive(true); // show the canvas when dialogue starts

        nameText.text = dialogue.name;
        sentences.Clear();
        foreach (var sentence in dialogue.sentences) sentences.Enqueue(sentence);
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();
        dialogueText.text = "";
        StartCoroutine(TypeSentence(currentSentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        foreach (var letter in sentence)
        {
            dialogueText.text += letter;
            yield return null;
        }

        isTyping = false;
    }

    public void EndDialogue()
    {
        FindObjectOfType<PlayerController>().CanMove = true;
        FindObjectOfType<PlayerController>().SetCanJump(true);
        FindObjectOfType<PlayerStateMachine>().ChangeState(new IdleState());

        dialogueCanvas.SetActive(false); // hide the canvas when dialogue ends

        foreach (var dialogueTrigger in dialogueTriggers) dialogueTrigger.EndDialogue();
    }
}