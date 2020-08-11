using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffect : MonoBehaviour, IPoolable
{
    public GameObject GameObject => gameObject;

    [SerializeField]
    private new ParticleSystem particleSystem = default;

    public Action OnDeactivate { get; set; }

    private void Awake()
    {
        if (!particleSystem)
            particleSystem = GetComponent<ParticleSystem>();
    }

    public void Activate(Vector2 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
        particleSystem.Play();
    }

    private void LateUpdate()
    {
        if (!particleSystem.isPlaying)
        {
            Deactivate();
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
