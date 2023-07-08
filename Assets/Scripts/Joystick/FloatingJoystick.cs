using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingJoystick : MonoBehaviour
{
    [HideInInspector]
    public RectTransform RectTransform; // Joystick'in RectTransform bileşeni
    public RectTransform Knob; // Joystick'in Knob bileşeni
    private void Awake() {
        RectTransform = GetComponent<RectTransform>(); // RectTransform bileşenini alır
    }
}
