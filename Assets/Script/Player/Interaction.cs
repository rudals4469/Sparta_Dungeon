using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float _lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    private IInteractable _curInteractable;

    public TextMeshProUGUI promptText;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        
        if (_camera == null)
            Debug.LogError("Main Camera not found! _camera is null.");

        GameObject promptObj = GameObject.Find("PromptText");
        if (promptObj != null)
        {
            promptText = promptObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogWarning("PromptText object not found in scene!");
        }
    }
    private void OnDrawGizmos()
    {
        if (_camera == null) return;

        Vector3 origin;
        Vector3 direction;
        float rayDistance;

        if (_camera == null) return;


        if (_camera.CompareTag("ThirdPersonCamera"))
        {

            origin = _camera.transform.position;
            direction = _camera.transform.forward;
            rayDistance = 10f;
        }
        else
        {
            origin = _camera.transform.position;
            direction = _camera.transform.forward;
            rayDistance = maxCheckDistance;
        }

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, rayDistance, layerMask))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(origin, hit.point);
            Gizmos.DrawSphere(hit.point, 0.05f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(origin, direction * rayDistance);
        }
    }

    private void Update()
    {
        
        if (_camera == null)
        {
            Debug.LogWarning("_camera is null in Update");
            return;
        }
        
        if (Time.time - _lastCheckTime > checkRate)
        {
            _lastCheckTime = Time.time;

            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
            RaycastHit hit;

            // 디버깅용
            Debug.DrawRay(ray.origin, ray.direction * maxCheckDistance, Color.red);

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    _curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                _curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = _curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && _curInteractable != null)
        {
            _curInteractable.OnInteract();
            curInteractGameObject = null;
            _curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
    
    public void UpdateCamera(Camera cam)
    {
        _camera = cam;
    }
    
    public void SetMaxCheckDistance(float distance)
    {
        maxCheckDistance = distance;
    }

}
