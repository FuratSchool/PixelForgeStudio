using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformUpDown : MonoBehaviour
{
    public float speed = 2.0f;
    public float distance = 10.0f;

    public string direction = "up";

    private Vector3 initialPosition;
    private bool movingUp = true;

    private void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        Vector3 targetPosition = initialPosition;

        if (movingUp && direction == "up")
        {
            targetPosition += Vector3.up * distance;
        }
        else if (!movingUp && direction == "down")
        {
            targetPosition += Vector3.down * distance;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            movingUp = !movingUp;
        }
    }
}
