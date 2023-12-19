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
        if(Input.GetKeyDown(KeyCode.K) && debugBoard)
            AddItemToBoard(testItem);
    }
    
    public void ActivateBoardUI()
    {
        FindObjectOfType<CameraController>().SetCameraActive(false);
        BoardUIActive = true;
        BoardUIobject.SetActive(true);
        if(_playerController.GetPlayerInput().currentControlScheme.Equals("Controller"))
        {
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
