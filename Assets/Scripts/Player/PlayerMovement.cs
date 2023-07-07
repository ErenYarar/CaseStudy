using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerMovement : MonoBehaviour
{
    // Rigidbody rb;
    // [SerializeField] [Range(0,5)] private float MaxSpeed = 2.75f;

    [Header("Joystick")]
    [SerializeField] Vector2 JoystickSize = new Vector2(300, 300);
    [SerializeField] FloatingJoystick Joystick;

    private Finger MovementFinger;
    private Vector2 MovementAmount;

    [Header("AI")]
    [SerializeField] private NavMeshAgent Player;

    private void Update()
    {
        Vector3 scaledMovement = Player.speed * Time.deltaTime * new Vector3(
            MovementAmount.x,
            0,
            MovementAmount.y
        );

        Player.transform.LookAt(transform.position + scaledMovement, Vector3.up);
        Player.Move(scaledMovement);
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void HandleFingerMove(Finger MovedFinger)
    {
        if (MovedFinger == MovementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = JoystickSize.x / 2f;
            ETouch.Touch currentTouch = MovedFinger.currentTouch;

            if (Vector2.Distance(
                    currentTouch.screenPosition,
                    Joystick.RectTransform.anchoredPosition
            ) > maxMovement)
            {
                knobPosition = (currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition;
            }

            Joystick.Knob.anchoredPosition = knobPosition;
            MovementAmount = new Vector2(-knobPosition.x, -knobPosition.y) / maxMovement;
            // MovementAmount = knobPosition / maxMovement;
        }
    }

    private void HandleLoseFinger(Finger LostFinger)
    {
        if (LostFinger == MovementFinger)
        {
            MovementFinger = null;
            Joystick.Knob.anchoredPosition = Vector2.zero;
            Joystick.gameObject.SetActive(false);
            MovementAmount = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger TouchedFinger)
    {
        // if (MovementFinger == null && TouchedFinger.screenPosition.x <= Screen.width / 2f) // Sol taraf
        if (MovementFinger == null)
        {
            MovementFinger = TouchedFinger;
            MovementAmount = Vector2.zero;
            Joystick.gameObject.SetActive(true);
            Joystick.RectTransform.sizeDelta = JoystickSize;
            Joystick.RectTransform.anchoredPosition = ClampStartPosition(TouchedFinger.screenPosition);
        }
    }

    private Vector2 ClampStartPosition(Vector2 StartPosition)
    {
        if (StartPosition.x < JoystickSize.x / 2)
        {
            StartPosition.x = JoystickSize.x / 2;
        }
        else if (StartPosition.x > Screen.width - JoystickSize.x / 2)
        {
            StartPosition.x = Screen.width - JoystickSize.x / 2;
        }

        if (StartPosition.y < JoystickSize.y / 2)
        {
            StartPosition.y = JoystickSize.y / 2;
        }
        else if (StartPosition.y > Screen.height - JoystickSize.y / 2)
        {
            StartPosition.y = Screen.height - JoystickSize.y / 2;
        }

        return StartPosition;
    }


    // private Vector2 ClampStartPosition(Vector2 StartPosition)
    // {
    //     if (StartPosition.x < JoystickSize.x / 2)
    //     {
    //         StartPosition.x = JoystickSize.x / 2;
    //     }

    //     if (StartPosition.y < JoystickSize.y / 2)
    //     {
    //         StartPosition.y = JoystickSize.y / 2;
    //     }
    //     else if (StartPosition.y > Screen.height - JoystickSize.y / 2)
    //     {
    //         StartPosition.y = Screen.height - JoystickSize.y / 2;
    //     }
    //     return StartPosition;
    // }
}