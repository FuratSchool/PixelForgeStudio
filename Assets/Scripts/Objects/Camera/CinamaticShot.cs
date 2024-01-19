using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]


public class CinamaticShot : MonoBehaviour
{
    [SerializeField] private Transform EndPosition;
    [SerializeField] private float PlayTime = 10;
    [SerializeField] private GameObject _screen;
    [SerializeField] private float ScreenOnTime = 1;

    [SerializeField] private GameObject Sign;
    [SerializeField] private GameObject Campfire;
    [SerializeField] private GameObject Chair;
    [SerializeField] private GameObject NPC;
    private float timePassed;
    public bool Once = false;
    private bool FadeOut = false;
    private bool FadeIn = false;
    private bool FadeInText = false;
    private bool waited = false;
    private bool CinematicStarted = false;
    [SerializeField] private bool FirstCinematic = false;
    [SerializeField] private bool SecondCinematic = false;
    [SerializeField] private string SceneName;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private GameObject CameraPos;
    private CinemachineFreeLook _freeLook;
    private PlayerController _playerController;
    private Vector3 _startPosition;
    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _freeLook = FindObjectOfType<CinemachineFreeLook>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(other.transform.parent != null) return;
            if (!Once && FirstCinematic)
            {
                Once = true;
                StartFade();
            }
            else if(!Once && SecondCinematic)
            {
                if(!NPC.GetComponent<DialogueTrigger>().hasBeenTalkedTo) return;
                _playerController.isTransitioning = true;
                Camera.main.GetComponent<CinemachineBrain>().enabled = false;
                StartCoroutine(Wait(PlayTime-1));
                CinematicStarted = true;
                GetComponentInChildren<CPC_CameraPath>().PlayPath(PlayTime);
            }
        }
    }
    
    IEnumerator Wait(float sec)
    {
        waited = false;
        yield return new WaitForSeconds(sec);
        waited = true;
    }

    void StartFade()
    {
        _playerController.isTransitioning = true;
        _screen.gameObject.SetActive(true);
        timePassed = 0;
        FadeIn = true;
        _screen.GetComponent<CanvasGroup>().alpha = 0;
    }
    private void Update()
    {
        if (FirstCinematic)
        {
            if (FadeIn)
            {
                timePassed += Time.deltaTime;
                if (timePassed < 1)
                {
                    _screen.GetComponent<CanvasGroup>().alpha = math.lerp(0, 1, timePassed);
                }
                else
                {
                    FadeIn = false;
                    FadeInText = true;
                    timePassed = 0;
                    _screen.GetComponent<CanvasGroup>().alpha = 1;

                    StartCoroutine(Wait(1));
                }
            }

            if (waited && FadeInText)
            {
                timePassed += Time.deltaTime;
                if (timePassed < 1)
                {
                    _screen.GetComponentInChildren<TMP_Text>().alpha = math.lerp(0, 1, timePassed);
                }
                else
                {
                    _screen.GetComponentInChildren<TMP_Text>().alpha = 1;
                    FadeInText = false;
                    FadeOut = true;
                    StartCoroutine(Wait(ScreenOnTime));
                }
            }

            if (waited && FadeOut)
            {
                if (timePassed < 1)
                {
                    timePassed -= Time.deltaTime;
                    _screen.GetComponent<CanvasGroup>().alpha = math.lerp(1, 0, timePassed);
                }
                else
                {
                    EnableCave();
                    FadeOut = false;
                    StartCinematic();
                }
            }

            if (waited && CinematicStarted)
            {
                if(Vector3.Distance(CameraPos.transform.position, Camera.main.transform.position) < 0.5f)
                {
                    CinematicStarted = false;
                    Camera.main.GetComponent<CinemachineBrain>().enabled = true;
                    _playerController.isTransitioning = false;
                }
                else
                {
                    Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation,CameraPos.transform.rotation, Time.deltaTime);
                    Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,CameraPos.transform.position, Time.deltaTime);
                }
            }
        }
        else if (SecondCinematic)
        {
            if (waited && CinematicStarted)
            {
                FindObjectOfType<SceneController>().LoadSceneAsync(SceneName);
                CinematicStarted = false;
                Camera.main.GetComponent<CinemachineBrain>().enabled = true;
                
                _playerController.isTransitioning = false;
            }
        }
    }

    private void EnableCave()
    {
        Sign.SetActive(true);
        Campfire.SetActive(true);
        Chair.SetActive(true);
        NPC.SetActive(true);
    }
    private void StartCinematic()
    {
        _screen.gameObject.SetActive(false);
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        _playerController.transform.position = EndPosition.position;
        _playerController.transform.rotation = EndPosition.rotation;
        _freeLook.m_YAxis.Update(Time.deltaTime);
        _freeLook.m_XAxis.Update(Time.deltaTime);
        _startPosition = Camera.main.transform.position;
        StartCoroutine(Wait(PlayTime+1));
        CinematicStarted = true;
        GetComponentInChildren<CPC_CameraPath>().PlayPath(PlayTime);
        timePassed = 0;
    }
}
