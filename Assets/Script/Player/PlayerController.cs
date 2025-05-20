using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    private Vector2 _curMovementInput;
    public LayerMask groundLayerMask;
    
    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float _camCurXRot;
    public float lookSensitivity;
    private Vector2 _mouseDelta;
    public bool canLook = true;

    [Header("Button Icon")]
    public Sprite mushroomIcon;
    public Sprite doubleJumpIcon;
    
    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float groundCheckOffset = 0.9f; // 플레이어 중심에서 아래로 얼마나 떨어진 위치에 체크할지
    [SerializeField] private bool showGroundGizmo = true; // 기즈모 표시 여부
    
    [Header("Stats")]
    public float jumpStaminaCost = 10f;
    public int maxJumpCount = 1; // 기본은 1단 점프
    private int _currentJumpCount;

    private Rigidbody _rigidbody;
    private WallClimbing _wallClimbing;
    private bool _wasGrounded;

    public Condition staminaBar;
    public Action Inventory;

    private void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        _rigidbody = GetComponent<Rigidbody>();
        _wallClimbing = GetComponent<WallClimbing>();
        _currentJumpCount = maxJumpCount;
    }
    private void FixedUpdate()
    {
        if (_wallClimbing != null && _wallClimbing.IsClimbing())
        {
            return;
        }

        bool isGrounded = IsGround();

        if (!_wasGrounded && isGrounded)
        {
            ResetJumpCount();
        }
        
        _wasGrounded = isGrounded;

        Move();
        
    }

    private void LateUpdate()
    {
        if (canLook && (_wallClimbing == null || !_wallClimbing.IsClimbing()))
        {
            CameraLook();
        }
    }

    void Move()
    {
        Vector3 dir = transform.forward * _curMovementInput.y + transform.right * _curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;
        
        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        _camCurXRot += _mouseDelta.y * lookSensitivity;
        _camCurXRot = Mathf.Clamp(_camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-_camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, _mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            TryJump();
        }
    }

    private void TryJump()
    {
        if (_wallClimbing != null && _wallClimbing.IsClimbing())
        {
            return;
        }

        if (_currentJumpCount > 0 && staminaBar.curValue >= jumpStaminaCost)
        {
            staminaBar.Subtract(jumpStaminaCost);
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z); // 기존 Y속도 제거
            _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            _currentJumpCount--;
        }
    }
    
    private void ResetJumpCount()
    {
        _currentJumpCount = maxJumpCount;
    }
    

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Inventory?.Invoke();
            ToggleCursor();
        }
    }

    public bool IsGround()
    {
        Vector3 spherePos = transform.position + Vector3.down * 0.9f;
        return Physics.CheckSphere(spherePos, 0.2f, groundLayerMask);
    }

    private void OnDrawGizmos()
    {
        if (!showGroundGizmo) return;

        Gizmos.color = Color.red;
        Vector3 checkPos = transform.position + Vector3.down * groundCheckOffset;
        Gizmos.DrawWireSphere(checkPos, groundCheckRadius);
    }

    public void BoostMoveSpeed(float multiplier, float duration)
    {
        BuffUIManager.Instance.ShowBuff(mushroomIcon, duration, BuffType.SpeedBoost);
        StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        float orninalSpeed = moveSpeed;
        moveSpeed *= multiplier;

        yield return new WaitForSeconds(duration);

        moveSpeed = orninalSpeed;
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
    
    public void EnableDoubleJump(bool CheckDbouleJump = false, float duration = 0f)
    {
        if (CheckDbouleJump)
        {
            BuffUIManager.Instance.ShowBuff(doubleJumpIcon, duration, BuffType.DoubleJump);
            StopCoroutine(nameof(TemporaryDoubleJump));
            StartCoroutine(TemporaryDoubleJump(duration));
        }
        else
        {
            maxJumpCount = 1;
            _currentJumpCount = maxJumpCount;
        }
    }

    private IEnumerator TemporaryDoubleJump(float duration)
    {
        maxJumpCount = 2;
        _currentJumpCount = maxJumpCount; // 더블 점프 시작 시 리셋

        yield return new WaitForSeconds(duration);

        maxJumpCount = 1;
        _currentJumpCount = maxJumpCount; // 더블 점프 끝난 후 다시 리셋
    }

}
