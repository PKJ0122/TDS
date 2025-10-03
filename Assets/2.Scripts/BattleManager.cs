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
        // ���� �� �� ������ ��� null ��ȯ
        if (_enemyLists.Count == 0)
        {
            return null;
        }

        IDamageHandler closestEnemy = null;
        float minDistanceSquared = float.PositiveInfinity; // �ּ� �Ÿ��� ���Ѵ�� �ʱ�ȭ

        Vector3 standardPosition = standard.position;

        foreach (var enemyHandler in _enemyLists)
        {
            // enemyHandler�� �������̽��̹Ƿ�, ���� ��ġ�� �������� Component�� ĳ�����ؾ� ��
            // ���� ���� �ı��Ǿ����� ����Ʈ���� ���� ���ŵ��� ���� ��츦 ����� null üũ
            Component enemyComponent = enemyHandler as Component;
            if (enemyComponent == null) continue;

            Transform enemyTransform = enemyComponent.transform;

            // ���� �Ÿ� ��� ���� �Ÿ��� ����Ͽ� ���� ����ȭ
            float distanceSquared = (enemyTransform.position - standardPosition).sqrMagnitude;

            // ��������� �ּ� �Ÿ����� �� ����� ���� �߰��ϸ�
            if (distanceSquared < minDistanceSquared)
            {
                // �ּ� �Ÿ��� ���� ����� ���� ����
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