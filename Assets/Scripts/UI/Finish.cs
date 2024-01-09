using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class Finish : MonoBehaviour
{
    private bool _hasFinished = false;
    private bool _once = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0;
            FindObjectOfType<UIController>().SetFinishActive(true);
            FindObjectOfType<UIController>().gameObject.GetComponent<Canvas>().sortingOrder = 1;
            _hasFinished = true;
        }
    }
    
    public void FinishGame()
    {
        StartCoroutine(Upload());
        Time.timeScale = 1;
        FindObjectOfType<SceneController>().LoadScene("MainMenu");
        
    }

    IEnumerator Upload()
    {
        UnityWebRequest response;
        string name = GameObject.Find("InputName").GetComponent<TMP_InputField>().text;
        string time = (Time.time - FindObjectOfType<SceneController>().TimePlayed).ToString("#.00");
        string deaths = FindObjectOfType<PlayerController>().DeathCount.ToString();
        string coins = FindObjectOfType<PlayerStatus>().Coins.ToString();
        using ( response = UnityWebRequest.Post(
                   "https://pixelforge-reaper-d850b-default-rtdb.europe-west1.firebasedatabase.app/" +
                   "LeaderBord/5fgozHfeHsjYnAPfTf7SYAhJfY2Zv98iUJJ3aI7jCRq10RpSk9.json", 
                   string.Format("{{\"Name\":\"{0}\"," +
                    "\"Time\":\"{1}\"," +
                    "\"Deaths\":\"{2}\"," +
                    "\"Coins\":\"{3}\"}}", name, time, deaths, coins)
                   ,"application/json"))
        {
            yield return response.SendWebRequest();

            if (response.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(response.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}