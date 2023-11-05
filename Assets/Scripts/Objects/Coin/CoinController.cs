using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private bool interacted = false;
    private Animation anim;
    private AudioSource audioSource;
    [SerializeField] AnimationClip coinAction;
    
    private void Start()
    {
         anim = gameObject.GetComponent<Animation>();
         anim.clip = coinAction;
         audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if (anim.isPlaying)
        { 
            return;
        }
        if(interacted && audioSource.isPlaying == false)
        {
            Destroy(gameObject);
        }
        anim.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerStatus>().AddCoin();
            audioSource.Play();
            GetComponent<MeshCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            interacted = true;
        }
    }
}