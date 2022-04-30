using UnityEngine;
using Cinemachine;
using RTS.Input;
using UnityEngine.InputSystem;

public class CameraHandler : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private PlayerInputActionMaps playerInputActionMaps;

    private float orthographicSize;
    private float targetOrthographicSize;

    private void Start()
    {
        playerInputActionMaps = InputManager.Instance.PlayerInputActionMap;

        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleZoom()
    {
        float zoomAmount = 2f / 120;
        targetOrthographicSize -= playerInputActionMaps.Gameplay.CameraZoom.ReadValue<float>() * zoomAmount;

        float minOrthographicSize = 5;
        float maxOrthographicSize = 30;
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);

        float zoomSpeed = 5f;
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }

    private void HandleMovement()
    {
        Vector2 input = playerInputActionMaps.Gameplay.CameraMovement.ReadValue<Vector2>();

        float moveSpeed = 10f;
        transform.position += (Vector3)input * moveSpeed * Time.deltaTime;
    }
}
