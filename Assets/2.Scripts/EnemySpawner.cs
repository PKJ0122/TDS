using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : SingletonMonoBase<EnemySpawner>
{
    public const string ENEMY_KEY = "enemy";
    public const string LANE_LAYER_NAME = "Lane";
    public const int NUMBER_OF_LANE = 3;

    [Tooltip("라인 간 시각적 Y축 간격")]
    public float laneOffsetY = 0.5f;

    public PoolObject enemyPrefab;

    public event Action<GameObject> OnEnemySpawn;


    protected override void Awake()
    {
        base.Awake();
        ObjectPool.Instance.CreatePool(ENEMY_KEY, enemyPrefab, 30);
    }

    IEnumerator Start()
    {
        WaitForSeconds _delay = WaitForSecondsCaching.Get(1.5f);

        for (int i = 0; i < 50; i++)
        {
            int count = Random.Range(0, NUMBER_OF_LANE);
            SpawnEnemy(count);
            yield return _delay;
        }
    }

    void SpawnEnemy(int laneIndex)
    {
        Vector3 spawnPosition = gameObject.transform.position;

        PoolObject enemy = ObjectPool.Instance.Get(ENEMY_KEY)
                                              .Get();

        enemy.transform.position = spawnPosition;
        enemy.GetComponent<EnemyAI>().SetLane(laneIndex);
        OnEnemySpawn?.Invoke(enemy.gameObject);
    }
}
