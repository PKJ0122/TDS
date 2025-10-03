using UnityEngine;

public class HpBarManager : SingletonMonoBase<HpBarManager>
{
    const string HPBAR_KEY = "Canvas - HpBar";

    [Tooltip("피사체와의 Y거리")]
    public float OffsetY;


    protected override void Awake()
    {
        base.Awake();
        PoolObject hpbarPrefab = Resources.Load<PoolObject>(HPBAR_KEY);
        ObjectPool.Instance.CreatePool(HPBAR_KEY, hpbarPrefab, 30);
        EnemySpawner.Instance.OnEnemySpawn += HpbarCreate;
    }

    private void OnDisable()
    {
        if (EnemySpawner.ApplicationQuit) return;

        EnemySpawner.Instance.OnEnemySpawn -= HpbarCreate;
    }

    void HpbarCreate(GameObject gameObject)
    {
        PoolObject hpbar = ObjectPool.Instance.Get(HPBAR_KEY)
                                              .Get();

        IHealth health = gameObject.GetComponent<IHealth>();

        hpbar.GetComponent<HpBar>().SetHpBar(health);
        hpbar.transform.SetParent(gameObject.transform,false);

        void DieHpbar()
        {
            hpbar.transform.SetParent(null);
            health.OnDie -= DieHpbar;
        }

        health.OnDie += DieHpbar;
        hpbar.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, OffsetY);
    }
}