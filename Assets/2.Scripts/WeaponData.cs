using System.Collections;
using UnityEngine;

public abstract class WeaponData : ScriptableObject
{
    public float BaseAtk;
    public float BaseCoolTime;
    public Bullet BulletPrefab;

    /// <summary>
    /// �����Լ�
    /// </summary>
    /// <param name="atkBuff">1.xx ������ �������</param>
    /// <param name="coolTimeBuff">1.xx ������ �������</param>
    public abstract IEnumerator C_Attack(Attacker attacker, float atkBuff, float coolTimeBuff);
}