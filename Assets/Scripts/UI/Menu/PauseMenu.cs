using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject firstSelectedButton;
    [SerializeField] private GameObject OptionsButton;
    [SerializeField] private GameObject UIObject;
    [SerializeField] private GameObject CoinsUI;
    [SerializeField] private BoardController _boardController;
    public  bool isPaused;
    
    private bool isOptionsOpen;
    private GameObject _player;

    private string ActiveControlScheme;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        optionsMenu.GetComponent<SettingsMenu>().InGameScene = true;
        _player = GameObject.Find("PlayerObject");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7)) //if the escape key is pressed
        {
            if (_boardController != null)
            {
                if (_boardController.BoardUIActive)
                {
                    _player.GetComponent<PlayerController>().InteractPressed = true;
                }
                else
                {
                    if(isPaused) //if the game is paused
                    {
                        if (isOptionsOpen)
                        {
                            CloseOptionsMenu();
                        }
                        else
                        {
                            ResumeGame(); //resumes the game
                        }
                    }
                    else
                    {
                        PauseGame(); //pauses the game
                    }
                }
            }
            else
            {
                if(isPaused) //if the game is paused
                {
                    if (isOptionsOpen)
                    {
                        CloseOptionsMenu();
                    }
                    else
                    {
                        ResumeGame(); //resumes the game
                    }
                }
                else
                {
                    PauseGame(); //pauses the game
                }
            }
        }
        
        if (isPaused && !isOptionsOpen)
        {
            if (EventSystem.current.currentSelectedGameObject == null &&
                _player.GetComponent<PlayerInput>().currentControlScheme.Equals("Controller"))
            {
                EventSystem.current.SetSelectedGameObject(firstSelectedButton);
            }
        } 
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        FindObjectOfType<UIController>().SetCoinAlpha(1);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        Time.timeScale = 0f; //stops the ingame time
        isPaused = true;
        CoinsUI.transform.SetParent(pauseMenu.transform);
    }

    public void ResumeGame()
    {
        FindObjectOfType<UIController>().SetCoinAlpha(0);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; //resumes the ingame time
        isPaused = false; 
        CoinsUI.transform.SetParent(UIObject.transform);
    }
    public void OpenOptionsMenu()
    {
        optionsMenu.SetActive(true);
        pauseMenu.transform.GetChild(0).gameObject.SetActive(false);
        isOptionsOpen = true;
        CoinsUI.SetActive(false);
    }
    
    public void CloseOptionsMenu()
    {
        optionsMenu.SetActive(false);
        pauseMenu.transform.GetChild(0).gameObject.SetActive(true);
        CoinsUI.SetActive(true);
        isOptionsOpen = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(OptionsButton);
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
