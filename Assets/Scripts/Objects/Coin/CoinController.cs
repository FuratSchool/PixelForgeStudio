using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class CoinController : MonoBehaviour
{
    [SerializeField] private float boost = 1.5f;
    [SerializeField] private float duration = 2f;
    private bool interacted = false;
    private AudioSource audioSource;
    private PlayerController _pc;
    
    private void Start()
    {
         audioSource = GetComponent<AudioSource>();
         _pc = FindObjectOfType<PlayerController>();
         
    }

    private void Update()
    {
        if(interacted && audioSource.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (interacted == false)
            {
                
                GetComponent<SphereCollider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                other.GetComponent<PlayerStatus>().AddCoin();
                var color = GetComponent<Renderer>().material.GetColor("_Color");
                _pc.StartSpeedBoost(boost,duration,color);
                audioSource.Play();
                interacted = true;
            }
        }
    }
}