using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingableObject : MonoBehaviour
{
    [SerializeField] private float swingDistance = 0f;
    [SerializeField] private float swingTime = 0f;
    [SerializeField] private float swingForce = 0f;

    private PlayerController _playerController;
    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(swingDistance > 0f){
                _playerController.swingDistance = swingDistance;
            }
            if(swingTime > 0f){
                _playerController.swingTime = swingTime;
            }
            if(swingForce > 0f){
                _playerController.exitForce = swingForce;
            }
            _playerController.inSwingingRange = true;
            _playerController.SwingableObjectGAME = transform.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerController.swingDistance = _playerController.standardSwingDistance;
            _playerController.swingTime = _playerController.standardSwingTime;
            _playerController.exitForce = _playerController.standardExitForce;
            _playerController.inSwingingRange = false;
        }
    }
}
