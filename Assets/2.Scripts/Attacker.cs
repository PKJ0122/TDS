using UnityEngine;

public class Attacker : MonoBehaviour
{
    [Tooltip("���� ������")]
    public WeaponData WeaponData;

    [Tooltip("���ݷ� ����")]
    public float AtkButt = 1f;

    [Tooltip("���ݼӵ� ����")]
    public float CooltimeBuff = 1f;

    [Tooltip("�ѱ�")]
    public Transform ShotPoint;


    public void Start()
    {
        StartCoroutine(WeaponData.C_Attack(this, AtkButt, CooltimeBuff));
    }
}
