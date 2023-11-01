using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void playGame(){
        SceneManager.LoadScene("SampleScene"); //goes to the next scene.
    }

    public void quitGame(){
        Debug.Log("Quit");
        Application.Quit();
    }
}
