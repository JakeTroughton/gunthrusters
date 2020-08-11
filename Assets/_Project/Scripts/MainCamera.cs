using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : Singleton<MainCamera>
{
    public new Camera camera;

    protected override void Awake()
    {
        base.Awake();
        if (!camera)
        {
            camera = GetComponent<Camera>();
        }
    }

    public Vector2 WorldToViewport(Vector2 position)
    {
        return camera.WorldToViewportPoint(position);
    }

    public Vector2 ViewportToWorld(Vector2 position)
    {
        return camera.ViewportToWorldPoint(position);
    }

    public bool IsOnScreen(Vector2 position)
    {
        Vector2 vPos = WorldToViewport(position);
        return vPos.x >= 0f && vPos.x <= 1f && vPos.y >= 0f && vPos.y <= 1f;
    }
}
