using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Swinging : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("LayerMask")]
    [SerializeField] private LayerMask swingable;
    
    [Header("Debug")]
    [SerializeField] private bool DebugGUI = false;
    
    [Header("Raycast")]
    [SerializeField] private float DistanceToObject = 10f;
    [SerializeField] private float angle = 0.2f;
    [SerializeField] private float sphereRadius = 1;
    
    [Header("SwingSettings")]
    [SerializeField] private float SwingDistance = 4f;
    [SerializeField] private float ExitForce = 4f;
    [SerializeField] private float SwingTime = 3f;
    [SerializeField] private float SwingDelay = 0.5f;
    [SerializeField] private LineRenderer lr;
    
    private bool SwingPressed = false;
    private bool _canSwing = true;
    private Vector3 collision;
    private bool _isSwinging;
    public bool IsSwinging => _isSwinging;
    public bool InRange { get; set; }

    private ConfigurableJoint joint;
    private Vector3 SwingableObjectPos;

    public GameObject player;
    // Update is called once per frame
    void Update()
    {
        if (_isSwinging == false && InRange)
        {
            CheckSwing();
        }
        else if (SwingPressed == false && _isSwinging)
        {
            EndSwing();
        }
    }
    
    private void CheckSwing()
    {
        var ray = new Ray(this.transform.position,
            (this.transform.forward.normalized + (this.transform.up * angle).normalized));
        RaycastHit hit;
        if (Physics.SphereCast(ray, sphereRadius, out hit, DistanceToObject, swingable) && _canSwing)
        {
            
            collision = hit.point;
            if (DebugGUI)
            {
                Debug.Log(hit.transform.gameObject);
                Debug.DrawLine(this.transform.position, collision, Color.green);
            }
            if(SwingPressed)
            {
                StartCoroutine(TimedSwing());

                SwingableObjectPos = hit.transform.gameObject.transform.position;
                MakeJoint(hit);
                lr.positionCount = 2;
                _isSwinging = true;
                _canSwing = false;
            }
        }
    }

    private void MakeJoint(RaycastHit hit)
    {
        joint = player.gameObject.AddComponent<ConfigurableJoint>();
        joint.autoConfigureConnectedAnchor = false;
        //joint.connectedBody = hit.rigidbody;
        joint.connectedAnchor = hit.transform.position;
        joint.anchor = new Vector3(0,0,0);
        
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;
        var limit = new SoftJointLimit();
        limit.limit = SwingDistance;
        limit.bounciness = 0f;
        limit.contactDistance = 0f;
        joint.linearLimit = limit;
    }

    private IEnumerator TimedSwing()
    {
        yield return new WaitForSeconds(SwingTime);
        if (joint != null)
        {
            EndSwing();
        }
        
        
    }

    private IEnumerator SwingDelayTimer()
    {
        yield return new WaitForSeconds(SwingDelay);
        _canSwing = true;
    }

    private void EndSwing()
    {
        lr.positionCount = 0;
        Destroy(joint);
        _isSwinging = false;
        player.GetComponent<Rigidbody>().AddForce(ExitForce * Vector3.up, ForceMode.Impulse);
        StartCoroutine(SwingDelayTimer());
    }
    private void LateUpdate()
    {
        if (!joint) return;
        lr.SetPosition(0, player.transform.position);
        lr.SetPosition(1, SwingableObjectPos);
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
