using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public float moveSpeed = 2f;
    public float waitTime = 1f;

    private Transform _target;
    private bool _isWaiting = false;

    private void Start()
    {
        _target = pointB;
    }

    private void Update()
    {
        if (_isWaiting || _target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, _target.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _target.position) < 0.01f)
        {
            StartCoroutine(SwitchTargetAfterWait());
        }
    }
    
    private IEnumerator SwitchTargetAfterWait()
    {
        _isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        _target = (_target == pointA) ? pointB : pointA;
        _isWaiting = false;
    }
}
