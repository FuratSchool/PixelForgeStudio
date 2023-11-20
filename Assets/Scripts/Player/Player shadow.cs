using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Playershadow : MonoBehaviour
{
    [SerializeField] private Transform shadow;
    
    public (float,Vector3) CheckGround()
    {
        var layermask = 1 << 6;
        Physics.Raycast(transform.position, Vector3.down,out var hit, 30f, ~layermask);
        var surface = hit.normal;
        var x = Mathf.Asin(hit.normal.x);
        var y = Mathf.Asin(hit.normal.y);
        var z = Mathf.Asin(hit.normal.z);
 
        //Debug.Log("Radians:" + x);
 
        //Debug.Log("Degrees:" + (x * Mathf.Rad2Deg)+ " y" + (y * Mathf.Rad2Deg)+ "z" + (z * Mathf.Rad2Deg));
        if(hit.point.y != null) return (hit.point.y,hit.normal);
        
        return (0f, new Vector3(0,0,0));

    }

    public void Update()
    {
        var normal = CheckGround().Item2;
        var angle = Vector3.Angle(shadow.transform.forward, normal);
        var rot = new Vector3(normal.x * Mathf.Rad2Deg,normal.y * Mathf.Rad2Deg,normal.z * Mathf.Rad2Deg);
        shadow.transform.position = new Vector3(shadow.position.x, CheckGround().Item1+.1f, shadow.position.z);
    }
}
