using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class CinamaticShot : MonoBehaviour
{
    [SerializeField] private Transform EndPosition;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(other.transform.parent != null) return;
            
            Camera.main.GetComponent<CinemachineBrain>().enabled = false;
            GameObject.Find("PlayerObject").transform.position = EndPosition.position;
            StartCoroutine(Wait(3));
            GetComponentInChildren<CPC_CameraPath>().PlayPath(3);
        }
    }
    
    IEnumerator Wait(float sec)
    {
        yield return new WaitForSeconds(sec);
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
    }
}
