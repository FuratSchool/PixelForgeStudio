using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    
    public static bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) //if the escape key is pressed
        {
            if(isPaused) //if the game is paused
            {
                ResumeGame(); //resumes the game
            }
            else
            {
                PauseGame(); //pauses the game
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; //stops the ingame time
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; //resumes the ingame time
        isPaused = false; 
    }
    public void OpenOptionsMenu()
    {
        optionsMenu.SetActive(true);
        pauseMenu.transform.GetChild(0).gameObject.SetActive(false);
    }
    
    public void CloseOptionsMenu()
    {
        optionsMenu.SetActive(false);
        pauseMenu.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; //resumes the ingame time
        SceneManager.LoadScene("MainMenu"); //loads the main menu scene
    }

    public void QuitGame()
    {
        Application.Quit(); //quits the game - only works in build
    }
}
