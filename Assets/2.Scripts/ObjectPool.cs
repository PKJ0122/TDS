using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : SingletonMonoBase<ObjectPool>
{
    Dictionary<string, IObjectPool<PoolObject>> _pool;

    public Dictionary<string, IObjectPool<PoolObject>> Pool
    {
        get
        {
            if (_pool == null)
            {
                _pool = new();
            }
            return _pool;
        }
    }


    protected override void Awake()
    {
        base.Awake();
    }

    public IObjectPool<PoolObject> Get(string key)
    {
        if (!Pool.ContainsKey(key)) return null;

        return Pool[key];
    }

    public void CreatePool(string key, PoolObject poolObject, int maxSize)
    {
        if (Pool.ContainsKey(key)) return;

        int capacity = Mathf.Min(maxSize, 20);
        IObjectPool<PoolObject> pool = new ObjectPool<PoolObject>(() => Create(key, poolObject),
                                                                        OnPoolItem,
                                                                        OnReleaseItem,
                                                                        OnDestroyItem,
                                                                        true,
                                                                        capacity,
                                                                        maxSize
                                                                        );
        Pool.Add(key, pool);
    }

    PoolObject Create(string key, PoolObject poolObject)
    {
        return Instantiate(poolObject).SetPool(Pool[key]);
    }

    public void OnPoolItem(PoolObject item)
    {
        item.gameObject.SetActive(true);
    }

    public void OnReleaseItem(PoolObject item)
    {
        item.gameObject.SetActive(false);
    }

    public void OnDestroyItem(PoolObject item)
    {
        Destroy(item.gameObject);
    }
}
