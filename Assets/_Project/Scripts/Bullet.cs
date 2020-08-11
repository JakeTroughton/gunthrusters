using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    public GameObject GameObject => gameObject;

    public Vector2 Direction { get; set; }
    [SerializeField]
    private float speed = 10f;

    private readonly float offscreenCheckMax = 0.5f;
    private float offscreenCheckCooldown;

    public Action OnDeactivate { get; set; }

    public void Activate(Vector2 position)
    {
        gameObject.SetActive(true);
        offscreenCheckCooldown = offscreenCheckMax;
        transform.localPosition = position;
    }

    private void Update()
    {
        Vector2 pos = transform.localPosition;
        pos += Direction * speed * Time.deltaTime;
        transform.localPosition = pos;

        if(offscreenCheckCooldown > 0f)
        {
            offscreenCheckCooldown -= Time.deltaTime;
        }
        else
        {
            if (MainCamera.Instance.IsOnScreen(transform.localPosition))
            {
                offscreenCheckCooldown = offscreenCheckMax;
            }
            else
            {
                Deactivate();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.CurrentHitpoints -= 1;
            Deactivate();
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

}
