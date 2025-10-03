using UnityEngine;
using UnityEngine.Pool;

public class PoolObject : MonoBehaviour
{
    IObjectPool<PoolObject> _myPool;

    public PoolObject SetPool(IObjectPool<PoolObject> pool)
    {
        _myPool = pool;
        return this;
    }

    public virtual void RelasePool()
    {
        _myPool.Release(this);
    }
}