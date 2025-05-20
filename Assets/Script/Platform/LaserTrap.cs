using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    public Transform laserOrigin; // 레이저가 발사되는 위치
    public Vector3 laserDirection = Vector3.forward; // 레이저 방향, 앞으로 초기화
    public float laserLength = 10f; // 레이저 길이
    public LayerMask playerLayerMask; // 플레이어만 감지
    public LineRenderer lineRenderer;
    
    [Header("UI")]
    public GameObject uILaser;
    public TMP_Text laserText;

    private bool _playerDetected = false; // 감지

    private void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        else
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, laserOrigin.position);
            lineRenderer.SetPosition(1, laserOrigin.position + laserDirection.normalized * laserLength);
        }
    }

    private void Update()
    {
        Ray ray = new Ray(laserOrigin.position, laserDirection.normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, laserLength, playerLayerMask))
        {
            if (!_playerDetected)
            {
                _playerDetected = true;
                OnPlayerEnter();
            }
        }
        else
        {
            if (_playerDetected)
            {
                _playerDetected = false;
                OnPlayerExit();
            }
        }

        if (lineRenderer != null)
        {
            Vector3 endPos = laserOrigin.position + laserDirection.normalized * laserLength;
            if (hit.collider != null)
            {
                endPos = hit.point;
            }
            
            lineRenderer.SetPosition(0, laserOrigin.position);
            lineRenderer.SetPosition(1, endPos);
        }
    }

    private void OnPlayerEnter()
    {
        if (uILaser != null)
        {
            uILaser.SetActive(true);
        }

        if (laserText != null)
        {
            laserText.text = "레이저에 맞았다";
        }
    }

    private void OnPlayerExit()
    {
        if (uILaser != null)
        {
            uILaser.SetActive(false);
        }

        if (laserText != null)
        {
            laserText.text = "";
        }
    }
    
    private void OnDrawGizmos()
    {
        if (laserOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(laserOrigin.position, laserDirection.normalized * laserLength);
        }
    }
    
}
