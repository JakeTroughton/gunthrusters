using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput = default;
    [SerializeField]
    private InputType side = InputType.Left;

    [SerializeField]
    private float linearRecoil = -1f;
    [SerializeField]
    private float angularRecoil = 1f;
    [SerializeField]
    private float fireRate = 0.15f;
    private float fireCooldown;

    // Floats are linear and angular recoil
    public event Action<float, float> OnFire;

    void Update()
    {
        if (fireCooldown <= 0f)
        {
            if((side & playerInput.CurrentInput) != 0)
            {
                fireCooldown = fireRate;
                OnFire?.Invoke(linearRecoil, angularRecoil);
            }
        }
        else
        {
            fireCooldown -= Time.deltaTime;
        }
    }
}
