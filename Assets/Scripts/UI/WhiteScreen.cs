using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class WhiteScreen : MonoBehaviour
{
    [Header("Fade")] 
    [SerializeField] private float FadeSpeed = .01f;
    [SerializeField] private float HoldSec = 2f;
    [SerializeField] private float FadeIteration = .01f;
    [SerializeField] private bool InRange = false;
    [SerializeField] public bool lockMovement = false;
    public Image canvas;
    public GameObject TeleportPosition;
    private GameObject player;
    private Vector3 _Pos;
    private PlayerController _playerController;
    private bool _fadeInStarted;
    private bool _fadeInFinished;
    private bool _fadeOutStarted;
    private bool _fadeOutFinished;
    private bool _isTransitioning;
    
    public bool isTransitioning { get { return _isTransitioning; } }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            if (other.transform.parent != null) return;
            Debug.Log(player);
            InRange = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InRange = false;
        }
    }

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        player = GameObject.FindObjectOfType<PlayerController>().gameObject;
        _Pos = TeleportPosition.transform.position;
    }

    private void Update()
    {
        if (InRange && _fadeInStarted == false)
        {
            StartCoroutine(FadeIn());
        }
        else if (_fadeInStarted && _fadeInFinished && _fadeOutStarted == false)
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        _playerController.isTransitioning = true;
        _isTransitioning = true;
        Debug.Log("Fade In!");
        _fadeInStarted = true;
        for (float alpha = 0; alpha <= 1; alpha += FadeIteration)
        {
            canvas.color = new Color(255,255,255,alpha);
            yield return new WaitForSeconds(FadeSpeed);
        }
        //teleport to level to
        player.transform.localPosition = _Pos;
        _fadeInFinished = true;
    }
    
    IEnumerator FadeOut()
    {
        Debug.Log("Fade Out!");
        _fadeOutStarted = true;
        _fadeInStarted = false;
        _fadeInFinished = false;
        yield return new WaitForSeconds(HoldSec);
        for (float alpha = 1; alpha >= 0; alpha -= FadeIteration)
        {
            canvas.color = new Color(255, 255, 255, alpha);
            yield return new WaitForSeconds(FadeSpeed);
        }

        _fadeOutStarted = false;
        _isTransitioning = false;
        _playerController.isTransitioning = false;
    }
}
