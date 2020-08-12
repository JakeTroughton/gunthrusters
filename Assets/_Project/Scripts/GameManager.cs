using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private float gameDuration = 30f;
    private float gameTimer;
    public float GameTimeNormalised => gameTimer / gameDuration;

    private bool isGameOver;

    [SerializeField]
    private float gameOverDuration = 2f;
    private float gameOverTimer = 2f;

    [Header("UI")]
    [SerializeField]
    private Text statusLabel = default;
    private int prevCountdownValue;

    protected override void Awake()
    {
        base.Awake();
        Time.timeScale = 1f;
        gameTimer = gameDuration;
        gameOverTimer = gameOverDuration;
    }

    void Start()
    {
        prevCountdownValue = (int)gameTimer;
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
                GameOver(true);
            }

            if (statusLabel && prevCountdownValue > gameTimer)
            {
                prevCountdownValue = (int)gameTimer;
                statusLabel.text = prevCountdownValue.ToString("00");
            }
        }
        else
        {
            gameOverTimer -= Time.unscaledDeltaTime;
            if(gameOverTimer <= 0f)
            {
                SceneManager.LoadScene(0);
                gameOverTimer = Mathf.Infinity;
            }
        }
    }

    public void GameOver(bool timeUp)
    {
        isGameOver = true;
        Time.timeScale = 0f;

        if (statusLabel)
        {
            if (timeUp)
            {
                statusLabel.text = "YOU\nWIN";
                statusLabel.color = new Color(0f, 1f, 0f, 0.3f);
            }
            else
            {
                statusLabel.text = "GAME\nOVER";
                statusLabel.color = new Color(1f, 0f, 0f, 0.3f);
            }
        }
    }
}
