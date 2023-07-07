using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    float _speed = 5f;
    float _rotateSpeed = 360f;
    float _verticalInput;
    float _horizontalInput;
    public bool isMoving = false;

    Vector3 cameraForward;
    Vector3 cameraRight;
    Vector3 input;

    private void Start()
    {
        isMoving = false;
        rb = GetComponent<Rigidbody>();

        cameraForward = Camera.main.transform.forward;
        cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();
    }
    void GatherInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        input = new Vector3(_horizontalInput, 0, _verticalInput);
        input *= -1; // Dikey girişi tersine çevir
    }

    void Look()
    {
        if (input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(input.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _rotateSpeed * Time.deltaTime);
    }
    private void Update()
    {
        GatherInput();
        Look();
    }

    private void FixedUpdate()
    {
        if (isMoving == false && _verticalInput != 0 || _horizontalInput != 0 && isMoving == false)
        {
            rb.MovePosition(transform.position + transform.forward * input.normalized.magnitude * _speed * Time.deltaTime);
        }
    }

}
