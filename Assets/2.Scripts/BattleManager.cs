using System.Collections.Generic;
using UnityEngine;

public class BattleManager : SingletonMonoBase<BattleManager>
{
    readonly HashSet<IDamageHandler> _enemyLists = new();


    protected override void Awake()
    {
        base.Awake();
        EnemySpawner.Instance.OnEnemySpawn += SpawnEnemy;
    }

    private void OnDisable()
    {
        if (EnemySpawner.ApplicationQuit) return;

        EnemySpawner.Instance.OnEnemySpawn -= SpawnEnemy;
    }

    public IDamageHandler FindClosestEnemy(Transform standard)
    {
        // 적이 한 명도 없으면 즉시 null 반환
        if (_enemyLists.Count == 0)
        {
            return null;
        }

        IDamageHandler closestEnemy = null;
        float minDistanceSquared = float.PositiveInfinity; // 최소 거리를 무한대로 초기화

        Vector3 standardPosition = standard.position;

        foreach (var enemyHandler in _enemyLists)
        {
            // enemyHandler는 인터페이스이므로, 실제 위치를 얻으려면 Component로 캐스팅해야 함
            // 만약 적이 파괴되었으나 리스트에서 아직 제거되지 않은 경우를 대비해 null 체크
            Component enemyComponent = enemyHandler as Component;
            if (enemyComponent == null) continue;

            Transform enemyTransform = enemyComponent.transform;

            // 실제 거리 대신 제곱 거리를 계산하여 성능 최적화
            float distanceSquared = (enemyTransform.position - standardPosition).sqrMagnitude;

            // 현재까지의 최소 거리보다 더 가까운 적을 발견하면
            if (distanceSquared < minDistanceSquared)
            {
                // 최소 거리와 가장 가까운 적을 갱신
                minDistanceSquared = distanceSquared;
                closestEnemy = enemyHandler;
            }
        }

        return closestEnemy;
    }


    void SpawnEnemy(GameObject enemy)
    {
        if (!enemy.TryGetComponent(out IDamageHandler handler)) return;

        _enemyLists.Add(handler);

        void RemoveEnemy()
        {
            _enemyLists.Remove(handler);
            handler.Health.OnDie -= RemoveEnemy;
        }

        handler.Health.OnDie += RemoveEnemy;
    }
}