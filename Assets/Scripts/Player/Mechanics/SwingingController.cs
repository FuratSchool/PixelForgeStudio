using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwingingController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("LayerMask")] [SerializeField] private LayerMask swingable;

    [Header("Debug")] [SerializeField] private bool DebugGUI;

    [Header("Raycast")] [SerializeField] private float DistanceToObject = 10f;

    [SerializeField] private float angle = 0.2f;
    [SerializeField] private float sphereRadius = 1;

    [Header("SwingSettings")] [SerializeField]
    private float SwingDistance = 4f;

    [SerializeField] private float ExitForce = 4f;
    [SerializeField] private float SwingTime = 3f;
    [SerializeField] private float SwingDelay = 0.5f;
    [SerializeField] private LineRenderer lr;

    public GameObject player;
    private bool _canSwing = true;
    private Vector3 collision;
    private Interactable interactable;

    private ConfigurableJoint joint;
    private Vector3 SwingableObjectPos;

    private bool SwingPressed;
    public bool IsSwinging { get; private set; }

    public bool InRange { get; set; }

    public GameObject SwingableObjectGAME { get; set; }

    // Update is called once per frame
    private void Start()
    {
        interactable = FindObjectOfType<Interactable>();
    }

    private void Update()
    {
        if (IsSwinging == false && InRange)
            CheckSwing();
        else if (SwingPressed == false && IsSwinging)
            EndSwing();
        else
            interactable.InteractableTextActive = false;
    }

    private void LateUpdate()
    {
        if (!joint) return;
        if (lr.positionCount != 0)
        {
            lr.SetPosition(0, player.transform.GetChild(1).transform.position);
            lr.SetPosition(1, SwingableObjectPos);
        }
    }

    private void OnDrawGizmos()
    {
        if (DebugGUI)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(collision, sphereRadius);
        }
    }

    private void CheckSwing()
    {
        if (_canSwing)
        {
            interactable.InteractText = "Hold E to Swing";
            interactable.InteractableTextActive = true;
            collision = SwingableObjectGAME.transform.position;
            if (DebugGUI)
                //Debug.Log(collision);
                Debug.DrawLine(transform.position, collision, Color.green);

            if (SwingPressed)
            {
                StartCoroutine(TimedSwing());

                SwingableObjectPos = SwingableObjectGAME.transform.gameObject.transform.position;
                MakeJoint(collision);
                lr.positionCount = 2;
                IsSwinging = true;
                _canSwing = false;
            }
        }
        else
        {
            interactable.InteractableTextActive = false;
        }
    }

    private void MakeJoint(Vector3 hit)
    {
        joint = player.gameObject.AddComponent<ConfigurableJoint>();
        joint.autoConfigureConnectedAnchor = false;
        //joint.connectedBody = hit.rigidbody;
        joint.connectedAnchor = hit;
        joint.anchor = new Vector3(0, 0, 0);

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
        if (joint != null) EndSwing();
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
        IsSwinging = false;
        player.GetComponent<Rigidbody>().AddForce(ExitForce * Vector3.up, ForceMode.Impulse);
        StartCoroutine(SwingDelayTimer());
    }

    private void OnSwing(InputValue input)
    {
        SwingPressed = Convert.ToBoolean(input.Get<float>());
    }
}