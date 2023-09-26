using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Swinging : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask swingable;
    public float DistanceToObject = 10f;
    public float sphereRadius = 1;
    public Vector3 collision = Vector3.zero;
    public float angle = 0.2f;
    public bool DebugGUI = false;
    public bool IsSwinging;
    private SpringJoint joint;
    private float maxSwingDistance = 10f;
    private bool SwingPressed = false;

    public GameObject player;
    // Update is called once per frame
    void Update()
    {
        if (SwingPressed && IsSwinging == false)
        {
            var ray = new Ray(this.transform.position,
                (this.transform.forward.normalized + (this.transform.up * angle).normalized));
            RaycastHit hit;
            if (Physics.SphereCast(ray, sphereRadius, out hit, DistanceToObject, swingable))
            {
                collision = hit.point;
                if (DebugGUI)
                {
                    Debug.Log(hit.transform.gameObject);
                    Debug.DrawLine(this.transform.position, collision, Color.green);
                }
                


                joint = player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = collision;

                float distanceFormPoint = Vector3.Distance(player.transform.position, collision);

                joint.maxDistance = distanceFormPoint * 0.5f;
                joint.minDistance = distanceFormPoint * 0.75f;

                joint.spring = 4.5f;
                joint.damper = 7f;
                joint.massScale = 1.5f;
                IsSwinging = true;
            }
        }
        else if (SwingPressed == false && IsSwinging)
        {
            Destroy(joint);
            IsSwinging = false;
        }
    }

    void OnSwing(InputValue input)
    {
        SwingPressed = Convert.ToBoolean(input.Get<float>());
    }
    void OnDrawGizmos()
    {
        if (DebugGUI)
        { 
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(collision, sphereRadius);
        }
    }
}
