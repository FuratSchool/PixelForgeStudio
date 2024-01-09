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
    private void Start()
    {
        IntroVideoPlayer.loopPointReached += EndReached;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F4))
        {
            EndCutscene();
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
    }
}
