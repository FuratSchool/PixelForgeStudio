using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private Animation anim;
    [SerializeField] AnimationClip coinAction;
    
    private void Start()
    {
         anim = gameObject.GetComponent<Animation>();
         anim.clip = coinAction;
         
    }

    private void Update()
    {
        if (anim.isPlaying)
        { 
            return;
        }
        anim.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            other.GetComponent<PlayerStatus>().AddCoin();
        }
    }
}