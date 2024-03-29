using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void playGame(){
        FindObjectOfType<SceneController>().LoadScene("TutorialSceneV1"); //goes to the next scene.
    }

    public void quitGame(){
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OptionsButtonHighLightEnable(GameObject Object)
    {
        Object.GetComponent<TMP_Text>().fontStyle |= FontStyles.Underline;
    }
    public void OptionsButtonHighLightDisable(GameObject Object)
    {
        
        Object.GetComponent<TMP_Text>().fontStyle ^= FontStyles.Underline;
    }
    
}
