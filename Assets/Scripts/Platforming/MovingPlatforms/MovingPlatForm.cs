using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatForm : MonoBehaviour
{
    public float speed = 2.0f;
    
    private Vector3 initialPosition;
    private bool movingForward = true;
    private Vector3 targetPosition;
    
    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = transform.parent.GetChild(0).position;
    }

    
    void Update()
    {
        if (movingForward)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                initialPosition,
                speed * Time.deltaTime
            );
        }
       

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f || 
            Vector3.Distance(transform.position, initialPosition) < 0.01f)
        {
            movingForward = !movingForward;
        }
    }
}
