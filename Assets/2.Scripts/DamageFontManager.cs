using UnityEngine;

public class DamageFontManager : SingletonMonoBase<DamageFontManager>
{
    const string DAMAGE_FONT_KEY = "Canvas - DamageFont";


    protected override void Awake()
    {
        base.Awake();
        PoolObject damageFontPrefab = Resources.Load<PoolObject>(DAMAGE_FONT_KEY);
        ObjectPool.Instance.CreatePool(DAMAGE_FONT_KEY, damageFontPrefab, 10);
    }

    public void CreateDamageFont(float damage, Transform damagedObject)
    {
        PoolObject damageFontObject = ObjectPool.Instance.Get(DAMAGE_FONT_KEY)
                                                         .Get();

        damageFontObject.GetComponent<DamageFont>().SetDamageFont(damage, damagedObject);
    }
}
