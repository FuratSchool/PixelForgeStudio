using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] private Transform shadow;
    
    public (float,Vector3) CheckGround()
    {
        var layermask = 1 << 6;
        Physics.Raycast(transform.position, Vector3.down,out var hit, 100f, ~layermask);
        
        if(hit.point.y != null) return (hit.point.y,hit.normal);

        return (0f, Vector3.back);

    }

    public void Update()
    {
        shadow.transform.localRotation = Quaternion.FromToRotation(Vector3.back, CheckGround().Item2);
        shadow.transform.position = new Vector3(shadow.position.x, CheckGround().Item1+.1f, shadow.position.z);
    }
}
