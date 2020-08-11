using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable
{
    public Transform Target { get; set; }
    public GameObject GameObject => gameObject;

    [SerializeField]
    private float linearSpeed = 5f;
    [SerializeField]
    private float angularSpeed = 10f;

    public Action OnDeactivate { get; set; }

    public void Activate(Vector2 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
    }

    private void Update()
    {
        // Rotate
        Vector2 targetPosition = Target.position;
        Vector3 myPosition = transform.position;
        float targetAngleDeg = Mathf.Atan2(targetPosition.y - myPosition.y, targetPosition.x - myPosition.x) * Mathf.Rad2Deg - 90f;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0f, 0f, targetAngleDeg), angularSpeed * Time.deltaTime);
        
        // Move
        transform.localPosition += transform.up * linearSpeed * Time.deltaTime;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

}
