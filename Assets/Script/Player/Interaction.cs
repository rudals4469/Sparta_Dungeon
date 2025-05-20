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

    public bool showGizmoz = true;
    private SimpleOutLine _lastOutlined;

    private void Start()
    {
        _camera = Camera.main;
        

        GameObject promptObj = GameObject.Find("PromptText");
        if (promptObj != null)
        {
            promptText = promptObj.GetComponent<TextMeshProUGUI>();
        }
    }
    private void OnDrawGizmos()
    {
        if (!showGizmoz ||_camera == null) return;

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
            return;
        }
        
        if (Time.time - _lastCheckTime > checkRate)
        {
            _lastCheckTime = Time.time;

            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * maxCheckDistance, Color.red);

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                GameObject hitObj = hit.collider.gameObject;

                if (hitObj != curInteractGameObject)
                {
                    // 이전 아웃라인 끄기
                    if (_lastOutlined != null)
                    {
                        _lastOutlined.DisableOutline();
                        _lastOutlined = null;
                    }

                    curInteractGameObject = hitObj;
                    _curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();

                    // 새 오브젝트 아웃라인 켜기
                    SimpleOutLine outline = hitObj.GetComponent<SimpleOutLine>();
                    if (outline != null)
                    {
                        outline.EnableOutline();
                        _lastOutlined = outline;
                    }
                }
            }
            else
            {
                // 레이 맞는 게 없으면 아웃라인 끄기
                if (_lastOutlined != null)
                {
                    _lastOutlined.DisableOutline();
                    _lastOutlined = null;
                }

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
