using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private bool interacted = false;
    private AudioSource audioSource;
    
    private void Start()
    {
         audioSource = GetComponent<AudioSource>();

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
                audioSource.Play();
                interacted = true;
            }
        }
    }
}