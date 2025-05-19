using UnityEngine;
using UnityEngine.InputSystem;

public class WallClimbing : MonoBehaviour
{
    [Header("Wall Climbing Settings")]
    public float climbSpeed = 1f; // 천천히 오르도록 낮은 값
    public float maxWallCheckDistance = 1f;
    public LayerMask wallLayerMask;

    private Rigidbody _rigidbody;
    private PlayerController _playerController;
    private bool _isClimbing = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerController = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (_isClimbing)
        {
            if (IsWallInFront())
            {
                ClimbWall();
            }
            else
            {
                StopClimbing();
            }
        }
    }

    private void ClimbWall()
    {
        Vector3 climbVelocity = Vector3.up * climbSpeed;
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, climbVelocity.y, _rigidbody.velocity.z);
    }

    public void OnClimb(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (IsWallInFront())
            {
                StartClimbing();
            }
        }
    }

    private void StartClimbing()
    {
        _isClimbing = true;
        _playerController.canLook = false;
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
    }

    private void StopClimbing()
    {
        _isClimbing = false;
        _playerController.canLook = true;
        _rigidbody.useGravity = true;
    }

    public bool IsClimbing()
    {
        return _isClimbing;
    }

    private bool IsWallInFront()
    {
        return Physics.Raycast(transform.position, transform.forward, maxWallCheckDistance, wallLayerMask);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, maxWallCheckDistance, wallLayerMask))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, hit.point);
            Gizmos.DrawSphere(hit.point, 0.05f);
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(origin, origin + direction * maxWallCheckDistance);
        }
    }
}
