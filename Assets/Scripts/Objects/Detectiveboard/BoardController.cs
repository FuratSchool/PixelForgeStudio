using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class BoardController : MonoBehaviour
{
    
    public bool isPlayerInRange;

    private UIController _uiController;
    private PlayerController _playerController;
    
    private bool KeyDebounced = true;
    public bool BoardUIActive { get; private set; }
    [SerializeField] private GameObject inGameObject;
    [SerializeField] private GameObject BoardUIobject;
    [SerializeField] private int MaxItemsOnBoard = 10;
    
    private int _currentItemsOnBoard;
    [Header("Debug Purposes")]
    public GameObject testItem;
    public bool debugBoard;
    private CinemachineFreeLook _freeLook;

    private float temp_x;
    private float temp_y;
    void Start()
    {
        _uiController = FindObjectOfType<UIController>();
        _playerController = FindObjectOfType<PlayerController>();
        
    }
    
    void Update()
    {
        CheckPlayerInrange();
        if(Input.GetKeyDown(KeyCode.K) && debugBoard)
            AddItemToBoard(testItem);
    }

    private void CheckPlayerInrange()
    {
        if (isPlayerInRange)
        {
            if (_playerController.InteractPressed && KeyDebounced)
            {
                StartCoroutine(DebounceKey());
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
    public void ActivateBoardUI()
    {
        FindObjectOfType<CameraController>().SetCameraActive(false);
        BoardUIActive = true;
        BoardUIobject.SetActive(true);
        inGameObject.GetComponentInChildren<BoardRange>()
            .DisableInteractActive(_uiController);
        if(_playerController.GetPlayerInput().currentControlScheme.Equals("Controller"))
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(BoardUIobject.transform.GetChild(0).GetChild(0).gameObject);
        }
    }
    
    public void DeactivateBoardUI()
    {
        FindObjectOfType<CameraController>().SetCameraActive(true);
        var tooltipobject = GameObject.FindGameObjectWithTag("ToolTip");
        if (tooltipobject != null)
        {
            Destroy(tooltipobject);
        }
        BoardUIActive = false;
        BoardUIobject.SetActive(false);
        if (isPlayerInRange)
        {
            inGameObject.GetComponentInChildren<BoardRange>()
                .EnableInteractActive(_uiController, _playerController.GetPlayerInput());
        }
    }
    IEnumerator DebounceKey()
    {
        KeyDebounced = false;
        yield return new WaitForSeconds(0.25f);
        KeyDebounced = true;
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
        var SpawnedItem = Instantiate(item, inGameObject.transform);
        SpawnedItem.transform.parent = BoardUIobject.transform.GetChild(0).transform;
        return SpawnedItem;
    }
}
