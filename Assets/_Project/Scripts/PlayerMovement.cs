using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera = default;
    [SerializeField]
    private new Rigidbody2D rigidbody2D = default;

    private List<PlayerGun> playerGuns = new List<PlayerGun>();

    void Awake()
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main;
        }

        if (!rigidbody2D)
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        playerGuns = new List<PlayerGun>(GetComponentsInChildren<PlayerGun>());
    }

    private void OnEnable()
    {
        foreach (var gun in playerGuns)
        {
            gun.OnFire += HandleGunFired;
        }
    }

    void LateUpdate()
    {
        WrapPosition();
    }

    void HandleGunFired(float linearRecoil, float angularRecoil)
    {
        rigidbody2D.AddTorque(angularRecoil, ForceMode2D.Impulse);
        rigidbody2D.AddForce(transform.up * linearRecoil, ForceMode2D.Impulse);
    }

    void WrapPosition()
    {
        Vector3 vPos = mainCamera.WorldToViewportPoint(transform.position);
        vPos.x = Mathf.Repeat(vPos.x, 1f);
        vPos.y = Mathf.Repeat(vPos.y, 1f);
        transform.position = mainCamera.ViewportToWorldPoint(vPos);
    }
}
