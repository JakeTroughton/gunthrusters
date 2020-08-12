using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    None    = 0,

    Left    = 1 << 0,
    Right   = 1 << 1,

    Both = Left | Right,
}

public class PlayerInput : MonoBehaviour
{
    public InputType CurrentInput { get; private set; }

    void Update()
    {
        CurrentInput = InputType.None;
#if UNITY_STANDALONE || UNITY_EDITOR
        if (Input.GetButton("Left"))
        {
            CurrentInput |= InputType.Left;
        }
        if (Input.GetButton("Right"))
        {
            CurrentInput |= InputType.Right;
        }
#elif UNITY_ANDROID
        for (int i = 0; i < Input.touchCount; i++)
        {
            float x = Input.GetTouch(i).position.x;
            if (x < Screen.width * 0.5f)
            {
                CurrentInput |= InputType.Left;
            }
            if (x > Screen.width * 0.5f)
            {
                CurrentInput |= InputType.Right;
            }
        }
#endif
    }
}
