using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhispGhostPatrol : MonoBehaviour
{
    [SerializeField] private GameObject[] _waypoints;
    [SerializeField] private float speed;
    [SerializeField] private float offset_swing = 0.5f;
    private WaypointTrigger _waypointTrigger;
    private int index = 0;
    private Vector3 TargetPos;
    private bool canMove = true;
    void Start()
    {
        index = 0;
        _waypointTrigger = _waypoints[index].GetComponent<WaypointTrigger>();
        _waypointTrigger.Unlock();
        TargetPos = _waypoints[index].transform.position;
        transform.position = _waypoints[index].transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_waypointTrigger.CanLock){
            if (_waypointTrigger.Triggered)
            {
                _waypointTrigger.Lock();
                index++;
                if (index >= _waypoints.Length)
                {
                    canMove = false;
                    StartCoroutine(Wait());
                }
                else
                {
                    _waypointTrigger = _waypoints[index].GetComponent<WaypointTrigger>();
                    _waypointTrigger.Unlock();
                    TargetPos = _waypointTrigger.transform.position;
                }
            }

        }
        else
        {
            if (Vector3.Distance(transform.position, TargetPos) < 0.1f)
            {
                _waypointTrigger.Lock();
                index++;
                if (index >= _waypoints.Length)
                {
                    canMove = false;
                    StartCoroutine(Wait());
                }
                else
                {
                    _waypointTrigger = _waypoints[index].GetComponent<WaypointTrigger>();
                    _waypointTrigger.Unlock();
                    TargetPos = _waypointTrigger.transform.position;
                }
            }
        }

        if (canMove)
        {
            var targetdir = Vector3.MoveTowards(transform.position, TargetPos, speed * Time.deltaTime);
            transform.position =
                new Vector3(targetdir.x + (Mathf.Sin(Time.time) * offset_swing),
                    targetdir.y + (Mathf.Cos(Time.time) * offset_swing),
                    targetdir.z);
        }
    }
    
    IEnumerator Wait(){
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
