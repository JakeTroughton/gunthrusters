using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Enemy enemyPrefab;
    private ObjectPool<Enemy> enemyPool;

    [SerializeField]
    private Transform chaseTarget;

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
        spawnLine = new Line(
            MainCamera.Instance.ViewportToWorld(new Vector2(0, 0)),
            MainCamera.Instance.ViewportToWorld(new Vector2(0, 1)),
            MainCamera.Instance.ViewportToWorld(new Vector2(1, 1)),
            MainCamera.Instance.ViewportToWorld(new Vector2(1, 0))
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
