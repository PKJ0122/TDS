using UnityEngine;

public class Attacker : MonoBehaviour
{
    [Tooltip("무기 데이터")]
    public WeaponData WeaponData;

    [Tooltip("공격력 버프")]
    public float AtkButt = 1f;

    [Tooltip("공격속도 버프")]
    public float CooltimeBuff = 1f;

    [Tooltip("총구")]
    public Transform ShotPoint;


    public void Start()
    {
        StartCoroutine(WeaponData.C_Attack(this, AtkButt, CooltimeBuff));
    }
}
