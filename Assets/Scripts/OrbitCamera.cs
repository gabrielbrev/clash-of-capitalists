using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;
    public float distance;
    public float orbitAngle;
    public float verticalAngle;
    public float speed;
    private Vector2 mouseDelta;
    private float targetOrbitAngle;

    void UpdatePosition()
    {
        Quaternion quaternion = Quaternion.Euler(verticalAngle, orbitAngle, 0f);
        Vector3 direction = quaternion * Vector3.forward;
        Vector3 position = target.position - direction * distance;

        transform.position = position;
        transform.LookAt(target);
    }

    void UpdateOrbit(float direction)
    {
        float amount = speed * Time.deltaTime;
        if (direction < 0)
        {
            orbitAngle = Math.Max(orbitAngle - amount, targetOrbitAngle);
        }
        else if (direction > 0)
        {
            orbitAngle = Math.Min(orbitAngle + amount, targetOrbitAngle);
        }
    }

    bool IsMouseInTopHalf()
    {
        return Mouse.current.position.ReadValue().y > Screen.height / 2;
    }

    bool IsMouseInRightHalf()
    {
        return Mouse.current.position.ReadValue().x > Screen.width / 2;
    }

    void HandleMouseDrag()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            float deltaAngle;
            float direction;

            mouseDelta = Mouse.current.delta.ReadValue();

            if (Math.Abs(mouseDelta.x) > Math.Abs(mouseDelta.y))
            {
                deltaAngle = mouseDelta.x;
                direction = IsMouseInTopHalf() ? -1f : 1f;
            }
            else
            {
                deltaAngle = mouseDelta.y;
                direction = IsMouseInRightHalf() ? 1f : -1f;
            }  

            targetOrbitAngle += deltaAngle * direction;
        }

        float angleDiff = targetOrbitAngle - orbitAngle;
        if (angleDiff != 0)
        {            
            UpdateOrbit(angleDiff);
        }
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
