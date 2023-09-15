using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformTopBottom : MonoBehaviour
{
    public float speed = 2.0f;
    public float distance = 10.0f;

    public string direction = "";

    private Vector3 initialPosition;
    private bool movingRight = true;

    private void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        Vector3 targetPosition = initialPosition;

        if (movingRight && direction == "top")
        {
            targetPosition += Vector3.right * distance;
        }
        else if (movingRight && direction == "down")
        {
            targetPosition += Vector3.left * distance;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            movingRight = !movingRight;
        }
    }
}
