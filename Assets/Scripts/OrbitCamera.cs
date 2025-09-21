using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;
    public float distance;
    public float verticalAngle;
    public float rotateSpeed;

    [SerializeField] private float orbitAngle;
    
    private float targetOrbitAngle;
    private Vector2 mouseDelta;
    private Vector2 mousePos;
    private Vector2 lastMousePos;

    private void UpdatePosition()
    {
        Quaternion quaternion = Quaternion.Euler(verticalAngle, orbitAngle, 0f);
        Vector3 direction = quaternion * Vector3.forward;
        Vector3 position = target.position - direction * distance;

        transform.position = position;
        transform.LookAt(target);
    }

    private bool IsMouseInTopHalf(Vector2 mousePos)
    {
        return mousePos.y > Screen.height / 2;
    }

    private bool IsMouseInRightHalf(Vector2 mousePos)
    {
        return mousePos.x > Screen.width / 2;
    }

    private void HandleMouseDrag()
    {
        mousePos = Mouse.current.position.ReadValue();
        
        if (Mouse.current.leftButton.isPressed)
        {
            float deltaAngle;
            float direction;

            mouseDelta = mousePos - lastMousePos;
            Debug.Log($"{mousePos} {lastMousePos} {mouseDelta}");

            // Decide a direção que a rotação vai acontecer dependendo do movimento e da posição do mouse
            if (Math.Abs(mouseDelta.x) > Math.Abs(mouseDelta.y))
            {
                deltaAngle = mouseDelta.x;
                direction = IsMouseInTopHalf(mousePos) ? -1f : 1f;
            }
            else
            {
                deltaAngle = mouseDelta.y;
                direction = IsMouseInRightHalf(mousePos) ? 1f : -1f;
            }

            targetOrbitAngle += deltaAngle * direction * rotateSpeed * Time.deltaTime;
        }
       
        lastMousePos = mousePos;
        orbitAngle = Mathf.LerpAngle(orbitAngle, targetOrbitAngle, Time.deltaTime * rotateSpeed);
    }

    void Start()
    {
        targetOrbitAngle = orbitAngle;
    }

    void Update()
    {
        HandleMouseDrag();
        UpdatePosition();
    }
}
