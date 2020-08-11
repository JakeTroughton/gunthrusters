using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private float gameDuration = 30f;
    private float gameTimer;

    public float GameTimeNormalised => gameTimer / gameDuration;

    private bool isGameOver;

    protected override void Awake()
    {
        base.Awake();
        gameTimer = gameDuration;
    }

    void Update()
    {
        if (!isGameOver)
        {
            if (gameTimer > 0f)
            {
                gameTimer -= Time.deltaTime;
            }
            else
            {
                gameTimer = 0f;
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        SceneManager.LoadScene(0);
    }
}
