using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    private Bullet bulletPrefab;
    private ObjectPool<Bullet> bulletPool;

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

    private void Awake()
    {
        bulletPool = new ObjectPool<Bullet>();
        bulletPool.Populate(bulletPrefab, 40, false);
    }

    void Update()
    {
        if (fireCooldown <= 0f)
        {
            if((side & playerInput.CurrentInput) != 0)
            {
                Fire();
            }
        }
        else
        {
            fireCooldown -= Time.deltaTime;
        }
    }

    void Fire()
    {
        if (bulletPool.TryActivate(transform.position, out Bullet bullet))
        {
            bullet.SetDirection(transform.up);
            OnFire?.Invoke(linearRecoil, angularRecoil);
        }
        fireCooldown = fireRate;
    }
}
