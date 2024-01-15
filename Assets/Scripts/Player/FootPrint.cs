using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.tag.Equals("Player"))
        {
            if (collision.gameObject.tag.Equals("platform"))
            {
                transform.SetParent(collision.transform);
            }
        }
    }
}
