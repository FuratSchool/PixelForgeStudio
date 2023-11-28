using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        BoardUIActive = true;
        BoardUIobject.SetActive(true);
        inGameObject.GetComponentInChildren<BoardRange>()
            .DisableInteractActive(_uiController);
    }
    
    public void DeactivateBoardUI()
    {
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
