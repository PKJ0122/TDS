using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalWeapon", menuName = "ScriptableObject/WeaponData/NormalWeapon")]
public class NormalWeapon : WeaponData
{
    public override IEnumerator C_Attack(Attacker attacker, float atkBuff, float coolTimeBuff)
    {
        ObjectPool.Instance.CreatePool(BulletPrefab.name, BulletPrefab.GetComponent<PoolObject>(), 5);
        float finalAtk = BaseAtk * atkBuff;
        float finalCooltime = BaseCoolTime / coolTimeBuff;
        WaitForSeconds waitForSeconds = WaitForSecondsCaching.Get(finalCooltime);

        while (true)
        {
            PoolObject bullet = ObjectPool.Instance.Get(BulletPrefab.name)
                                                   .Get();

            IDamageHandler target = BattleManager.Instance.FindClosestEnemy(attacker.transform);

            bullet.transform.position = attacker.ShotPoint.position;
            bullet.GetComponent<Bullet>().Shot(finalAtk, target);

            yield return waitForSeconds;
        }
    }
}
