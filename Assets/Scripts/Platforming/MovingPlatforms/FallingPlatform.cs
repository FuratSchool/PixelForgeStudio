using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshCollider))]
public class FallingPlatform : MonoBehaviour
{
    
    [SerializeField] private float _fallDelay = 1.5f;
    [SerializeField] private float _respawnDelay = 5f;
    [SerializeField] private float _shakeStrength = 1f;
    [SerializeField] private bool Rotate = false;
    private Vector3 _startPos;
    private Quaternion _startRot;
    private Rigidbody _rb;
    void Start()
    {
        _startPos = transform.position;
        _startRot = transform.rotation;
        _rb = GetComponent<Rigidbody>();
        if(Rotate)
            _rb.freezeRotation = false;
        else
            _rb.freezeRotation = true;
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
        transform.DOShakePosition(_fallDelay, _shakeStrength);
        yield return new WaitForSeconds(_fallDelay);
        _rb.isKinematic = false;
        yield return new WaitForSeconds(_respawnDelay);
        _rb.isKinematic = true;
        transform.position = _startPos;
        transform.rotation = _startRot;
    }
}
