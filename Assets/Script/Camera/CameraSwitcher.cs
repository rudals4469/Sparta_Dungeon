using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    private Camera _currentCamera;

    public Interaction interaction;

    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        SetCamera(firstPersonCamera);
    }

   
    public void OnSwitchToFirstPerson(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetCamera(firstPersonCamera);
        }
    }

    public void OnSwitchToThirdPerson(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetCamera(thirdPersonCamera);
        }
    }

    private void SetCamera(Camera cam)
    {
        if (_currentCamera != null)
            _currentCamera.gameObject.SetActive(false);

        _currentCamera = cam;
        _currentCamera.gameObject.SetActive(true);

        if (interaction != null)
        {
            interaction.UpdateCamera(_currentCamera);

            if (cam == firstPersonCamera)
                interaction.SetMaxCheckDistance(4f);
            else if (cam == thirdPersonCamera)
                interaction.SetMaxCheckDistance(8f);
        }
    }
}