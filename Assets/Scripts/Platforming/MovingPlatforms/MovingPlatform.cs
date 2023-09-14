using UnityEngine;

//To do Add direction support
public class MovePlatform : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDistance = 10f;

    public string direction = "";

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool movingForward = true;

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition =
            initialPosition + Vector3.forward * moveDistance + Vector3.up * moveDistance;
    }

    private void Update()
    {
        if (movingForward)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPosition) < 1f)
            {
                movingForward = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                initialPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, initialPosition) < 1f)
            {
                movingForward = true;
            }
        }
    }
}
