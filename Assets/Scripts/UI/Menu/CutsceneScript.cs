using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutsceneScript : MonoBehaviour
{
    [SerializeField] private GameObject _MainMenu;
    [SerializeField] private VideoPlayer IntroVideoPlayer;
    [SerializeField] private VideoPlayer LoopVideoPlayer;
    [SerializeField] private RenderTexture loopRenderTexture;
    private bool _ended;
    private float time = 0;
    private void Start()
    {
        IntroVideoPlayer.loopPointReached += EndReached;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F4) && _ended == false)
        {
            EndCutscene();
        }

        if (_ended && time < 1)
        {
            time += Time.deltaTime * 0.5f;
            _MainMenu.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, time);
        }
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        EndCutscene();
    }

    void EndCutscene()
    {
        GetComponentInChildren<RawImage>().texture = loopRenderTexture;
        LoopVideoPlayer.Play();
        IntroVideoPlayer.time = 0;
        IntroVideoPlayer.enabled = false;
        _MainMenu.SetActive(true);
        _ended = true;
        var buttons = FindObjectsOfType<Button>();
        foreach (var button in buttons)
        {
            button.interactable = true;
        }
    }
}
