using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private float gameDuration = 30f;
    private float gameTimer;

    public float GameTimeNormalised => gameTimer / gameDuration;

    protected override void Awake()
    {
        base.Awake();
        gameTimer = gameDuration;
    }

    void Update()
    {
        if (gameTimer > 0f)
        {
            gameTimer -= Time.deltaTime;
        }
        else
        {
            gameTimer = 0f;
        }
    }
}
