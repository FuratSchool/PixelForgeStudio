using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class WaypointTrigger : MonoBehaviour
{
    private bool _unlocked = false;
    private bool _triggered = false;
    [SerializeField] private bool _canLock = true;
    
    [SerializeField] private bool SceneCoupled = false;
    [SerializeField] private GameObject _scene;
    public bool Triggered
    {
        get => _triggered;
        set => _triggered = value;
    }
    
    public bool CanLock
    {
        get => _canLock;
        set => _canLock = value;
    }
    
    private WhispGhostPatrol _whispGhostPatrol;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            if (_unlocked)
            {
                Triggered = true;
                if (SceneCoupled)
                {
                    Destroy(_scene);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            Triggered = false;
        }
    }

    public void Unlock()
    {
        _unlocked = true;
        if (SceneCoupled)
        {
            _scene.SetActive(true);
        }
    }
    
    public void Lock()
    {
        _unlocked = false;
    }
}
