using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    
    public bool isPlayerInRange;

    private UIController _uiController;
    private PlayerController _playerController;
    
    private bool KeyDebounced = true;
    public bool BoardUIActive { get; private set; }
    [SerializeField] private GameObject inGameObject;
    [SerializeField] private GameObject BoardUIobject;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private int MaxItemsOnBoard = 10;
    
    private int _currentItemsOnBoard;
    private CinemachineFreeLook _freeLook;

    private float temp_x;
    private float temp_y;
    void Start()
    {
        _uiController = FindObjectOfType<UIController>();
        _playerController = FindObjectOfType<PlayerController>();
    }
    public void ActivateBoardUI()
    {
        if(_pauseMenu.isPaused) return;
        FindObjectOfType<CameraController>().SetCameraActive(false);
        BoardUIActive = true;
        _uiController.transform.GetComponent<Canvas>().sortingOrder = 1;
        BoardUIobject.SetActive(true);
        var rect = BoardUIobject.transform.GetChild(0).GetComponent<RectTransform>().rect;
        if (rect.height < 500 && rect.width < 500)
        {
            BoardUIobject.transform.GetChild(0).GetComponent<GridLayoutGroup>().cellSize = new Vector2(50, 50);
            foreach (var child in BoardUIobject.transform.GetChild(0).transform)
            {
                var childObject = (Transform) child;
                childObject.GetComponent<Tooltip>().TextSize = 10;
            }
        }
        else if (rect.height < 500 && rect.width > 500)
        {
            BoardUIobject.transform.GetChild(0).GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100);
            foreach (var child in BoardUIobject.transform.GetChild(0).transform)
            {
                var childObject = (Transform) child;
                childObject.GetComponent<Tooltip>().TextSize = 20;
            }
        }
        else if (rect.height > 1000 && rect.width > 1000)
        {
            foreach (var child in BoardUIobject.transform.GetChild(0).transform)
            {
                var childObject = (Transform) child;
                childObject.GetComponent<Tooltip>().TextSize = 60;
            }
            BoardUIobject.transform.GetChild(0).GetComponent<GridLayoutGroup>().cellSize = new Vector2(400, 400);
        }
        else
        {
            foreach (var child in BoardUIobject.transform.GetChild(0).transform)
            {
                var childObject = (Transform) child;
                childObject.GetComponent<Tooltip>().TextSize = 36;
            }
            BoardUIobject.transform.GetChild(0).GetComponent<GridLayoutGroup>().cellSize = new Vector2(250, 250);
        }
        if(_playerController.GetPlayerInput().currentControlScheme.Equals("Controller"))
        {
            EventSystem.current.SetSelectedGameObject(BoardUIobject.transform.GetChild(0).GetChild(0).gameObject);
            BoardUIobject.transform.GetChild(1).gameObject.SetActive(true);
            foreach (var child in BoardUIobject.transform.GetChild(0).transform)
            {
                var childObject = (Transform) child;
                childObject.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            BoardUIobject.transform.GetChild(1).gameObject.SetActive(false);
            foreach (var child in BoardUIobject.transform.GetChild(0).transform)
            {
                var childObject = (Transform) child;
                childObject.GetComponent<Button>().interactable = false;
            }
        }

        
    }
    
    public void DeactivateBoardUI()
    {
        _uiController.transform.GetComponent<Canvas>().sortingOrder = 0;
        FindObjectOfType<CameraController>().SetCameraActive(true);
        var tooltipobject = GameObject.FindGameObjectWithTag("ToolTip");
        if (tooltipobject != null)
        {
            Destroy(tooltipobject);
        }
        BoardUIActive = false;
        BoardUIobject.SetActive(false);
    }
    
    public GameObject AddItemToBoard(GameObject item)
    {
        if (_currentItemsOnBoard >= MaxItemsOnBoard)
        {
            Debug.Log("Board is full");
            return null;
        }
        else
        {
            _currentItemsOnBoard++;
        }
        var SpawnedItem = Instantiate(item, inGameObject.transform.position, Quaternion.identity);
        SpawnedItem.transform.SetParent(BoardUIobject.transform.GetChild(0).transform);
        return SpawnedItem;
    }

    public void InInteractState(PlayerController pc)
    {
        if (isPlayerInRange)
        {
            if (BoardUIActive)
            {
                DeactivateBoardUI();
            }
            else
            {
                ActivateBoardUI();
            }
        }
    }
}
