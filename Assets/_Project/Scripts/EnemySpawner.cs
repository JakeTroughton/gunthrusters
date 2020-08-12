using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Enemy enemyPrefab = default;
    private ObjectPool<Enemy> enemyPool;

    [SerializeField]
    private Transform chaseTarget = default;

    private Line spawnLine;

    [SerializeField]
    private float spawnRateMin = 3f;
    [SerializeField]
    private float spawnRateMax = 8f;
    private float spawnCooldown;

    private void Awake()
    {
        enemyPool = new ObjectPool<Enemy>();
        enemyPool.Populate(enemyPrefab, 10, false);

        spawnCooldown = spawnRateMin;
    }

    private void Start()
    {
        float min = -0.1f;
        float max = 1.1f;
        spawnLine = new Line(
            MainCamera.Instance.ViewportToWorld(new Vector2(min, min)),
            MainCamera.Instance.ViewportToWorld(new Vector2(min, max)),
            MainCamera.Instance.ViewportToWorld(new Vector2(max, max)),
            MainCamera.Instance.ViewportToWorld(new Vector2(max, min))
            );
    }

    void Update()
    {
        if(spawnCooldown > 0f)
        {
            spawnCooldown -= Time.deltaTime;
        }
        else
        {
            SpawnEnemy();
            spawnCooldown = Mathf.Lerp(spawnRateMin, spawnRateMax, GameManager.Instance.GameTimeNormalised);
        }
    }

    void SpawnEnemy()
    {
        Vector2 position = spawnLine.GetNormalisedPoint(Random.value);
        if (enemyPool.TryActivate(position, out Enemy enemy))
        {
            enemy.Target = chaseTarget;
            enemy.RotateToTarget(Mathf.Infinity);
        }
    }
}
