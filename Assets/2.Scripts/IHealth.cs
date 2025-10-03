using System;

public interface IHealth
{
    float MaxHp { get; set; }

    event Action<float> OnHealthChange;
    event Action OnDie;

    void TakeDamage(float damage);
}