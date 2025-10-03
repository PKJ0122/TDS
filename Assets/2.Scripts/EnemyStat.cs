using System;
using UnityEngine;

public class EnemyStat : MonoBehaviour, IHealth
{
    [Tooltip("���� �⺻ �ִ�ü��")]
    public float BaseMaxHp;

    public float MaxHp { get; set; }
    public float Hp { get; private set; }

    public event Action<float> OnHealthChange;
    public event Action OnDie;

    PoolObject _pool;


    void Awake()
    {
        _pool = GetComponent<PoolObject>();
    }

    void OnEnable()
    {
        //�������� �����Ȳ�� ü�� �ٸ��� �ϱ�����
        MaxHp = BaseMaxHp;
        Hp = BaseMaxHp;
    }

    public void TakeDamage(float damage)
    {
        Hp -= damage;
        OnHealthChange?.Invoke(Hp);

        if (Hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDie?.Invoke();

        if (_pool != null) _pool.RelasePool();
    }
}
