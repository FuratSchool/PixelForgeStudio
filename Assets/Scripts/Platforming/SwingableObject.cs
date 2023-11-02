using UnityEngine;

public class SwingableObject : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<Swinging>().InRange = true;
        FindObjectOfType<Swinging>().SwingableObjectGAME = transform.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        FindObjectOfType<Swinging>().InRange = false;
    }
}