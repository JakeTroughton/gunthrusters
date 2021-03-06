﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable
{
    public Transform Target { get; set; }
    public GameObject GameObject => gameObject;
    [SerializeField]
    private TrailRenderer trailRenderer;

    [SerializeField]
    private int maxHitpoints = 1;
    public int CurrentHitpoints { get; set; }
    [SerializeField]
    private float linearSpeed = 5f;
    [SerializeField]
    private float angularSpeed = 10f;

    public Action OnDeactivate { get; set; }

    public void Activate(Vector2 position)
    {
        CurrentHitpoints = maxHitpoints;
        gameObject.SetActive(true);
        transform.position = position;

        trailRenderer.Clear();
    }

    private void Update()
    {
        // Rotate
        RotateToTarget(angularSpeed);
        // Move
        transform.localPosition += transform.up * linearSpeed * Time.deltaTime;

        if(CurrentHitpoints <= 0)
        {
            Deactivate();
        }
    }

    public void RotateToTarget(float maxDegreesDelta)
    {
        Vector2 targetPosition = Target.position;
        Vector3 myPosition = transform.position;
        float targetAngleDeg = Mathf.Atan2(targetPosition.y - myPosition.y, targetPosition.x - myPosition.x) * Mathf.Rad2Deg - 90f;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0f, 0f, targetAngleDeg), maxDegreesDelta * Time.deltaTime);
    }

    public void Deactivate()
    {
        VisualEffectManager.Instance.ActivateBurst(transform.position);
        gameObject.SetActive(false);

        trailRenderer.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerMovement playerMovement = collider.GetComponent<PlayerMovement>();
        if (playerMovement)
        {
            GameManager.Instance.GameOver(false);
        }
    }
}
