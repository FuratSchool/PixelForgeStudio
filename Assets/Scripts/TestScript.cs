using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public float moveSpeed = 2.0f; // Speed of movement

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        float horizontalMovement = Mathf.Sin(Time.time) * moveSpeed;
        float verticalMovement = Mathf.Cos(Time.time) * moveSpeed;
        float upwardMovement = Mathf.Sin(Time.time * 1f) * moveSpeed;

        Vector3 movement =
            new Vector3(horizontalMovement, upwardMovement, verticalMovement) * Time.deltaTime;

        transform.Translate(movement);
    }
}
