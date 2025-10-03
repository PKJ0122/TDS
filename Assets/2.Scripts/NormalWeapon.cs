using DG.Tweening;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalWeapon", menuName = "ScriptableObject/WeaponData/NormalWeapon")]
public class NormalWeapon : WeaponData
{
    [Header("반동 설정")]
    [Tooltip("반동 시 뒤로 밀려날 거리")]
    public float recoilDistance = -0.5f;

    [Tooltip("반동 시간")]
    public float recoilDuration = 0.1f;


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
            TriggerRecoil(attacker);

            yield return waitForSeconds;
        }
    }

    public void TriggerRecoil(Attacker attacker)
    {
        attacker.Weapon.DOKill();

        Vector3 originalWeaponPosition = attacker.Weapon.position;

        attacker.Weapon.DOMoveX(originalWeaponPosition.x + recoilDistance, recoilDuration / 2)
                       .SetEase(Ease.OutQuad)
                       .SetLoops(2, LoopType.Yoyo);
    }
}
