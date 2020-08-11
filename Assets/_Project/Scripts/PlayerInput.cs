using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera = default;
    [SerializeField]
    private new Rigidbody2D rigidbody = default;
    [SerializeField]
    private float linearRecoil = 10f;
    [SerializeField]
    private float angularRecoil = 10f;
    [SerializeField]
    private float fireRate = 0.15f;
    private float fireCooldown;

    void Update()
    {
        if(fireCooldown <= 0f)
        {
            float linear = 0f;
            float angular = 0f;
            bool hasFired = false;

            if (Input.GetButton("Left"))
            {
                angular -= angularRecoil;
                linear = linearRecoil;
                hasFired = true;
            }

            if (Input.GetButton("Right"))
            {
                angular += angularRecoil;
                linear = linearRecoil;
                hasFired = true;
            }

            if (hasFired)
            {
                fireCooldown = fireRate;
                rigidbody.AddTorque(angular, ForceMode2D.Impulse);
                rigidbody.AddForce(transform.up * linear * -1f, ForceMode2D.Impulse);
            }
        }
        else
        {
            fireCooldown -= Time.deltaTime;
        }

        WrapPosition();
    }

    void WrapPosition()
    {
        Vector3 vPos = mainCamera.WorldToViewportPoint(transform.position);
        vPos.x = Mathf.Repeat(vPos.x, 1f);
        vPos.y = Mathf.Repeat(vPos.y, 1f);
        transform.position = mainCamera.ViewportToWorldPoint(vPos);
    }
}
