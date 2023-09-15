using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatFormLeftRight : MonoBehaviour
{
    public float speed = 2.0f;
    public float distance = 10.0f;

    public string direction = "";

    private Vector3 initialPosition;
    private bool movingForward = true;

    private Vector3 backwardDirection = -Vector3.forward;

    private void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        Vector3 targetPosition = initialPosition;

        if (direction == "left")
        {
            targetPosition += movingForward ? Vector3.forward * distance : Vector3.zero;
        }
        else if (direction == "right")
        {
            targetPosition += movingForward ? backwardDirection * distance : Vector3.zero;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            movingForward = !movingForward;
        }
    }
}
