using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VennToScythe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<DialogueTrigger>().hasBeenTalkedTo)
        {
            Destroy(gameObject);
        }
    }
}
