using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float _fallDelay = 1.5f;
    [SerializeField] private float _respawnDelay = 5f;
    private Vector3 _startPos;
    private Rigidbody _rb;
    void Start()
    {
        _startPos = transform.position;
        _rb = GetComponent<Rigidbody>();
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(_rb.isKinematic)
                StartCoroutine(Fall());
        }
    }
    
    IEnumerator Fall()
    {
        yield return new WaitForSeconds(_fallDelay);
        _rb.isKinematic = false;
        yield return new WaitForSeconds(_respawnDelay);
        _rb.isKinematic = true;
        transform.position = _startPos;
    }
}
