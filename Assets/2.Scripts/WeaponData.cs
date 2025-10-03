using System.Collections;
using UnityEngine;

public abstract class WeaponData : ScriptableObject
{
    public float BaseAtk;
    public float BaseCoolTime;
    public Bullet BulletPrefab;

    /// <summary>
    /// 공격함수
    /// </summary>
    /// <param name="atkBuff">1.xx 형식의 배수기입</param>
    /// <param name="coolTimeBuff">1.xx 형식의 배수기입</param>
    public abstract IEnumerator C_Attack(Attacker attacker, float atkBuff, float coolTimeBuff);
}