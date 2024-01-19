using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
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
            var sound = GetComponentInParent<AudioSource>();
            sound.Play();
            _boardController.AddItemToBoard(_ArtefactModel);
            Destroy(gameObject);
        }
    }
}
