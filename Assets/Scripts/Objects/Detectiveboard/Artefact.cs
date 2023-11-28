using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private GameObject _ArtefactModel;
    
    private BoardController _boardController;
    // Start is called before the first frame update
    void Start()
    {
        _boardController = FindObjectOfType<BoardController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _boardController.AddItemToBoard(_ArtefactModel);
            Destroy(gameObject);
        }
    }
}
