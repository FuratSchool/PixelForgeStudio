using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotateNPC : MonoBehaviour
{
    private bool _inRange;
    private GameObject _player;
    private float rotationSpeed = 5f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player = other.gameObject;
            _inRange = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player = null;
            _inRange = false;
        }
    }
    
    void Update()
    {
        if (_inRange)
        {
            if(_player == null) return; 
            Vector3 direction = _player.transform.position - transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(-direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }
}
