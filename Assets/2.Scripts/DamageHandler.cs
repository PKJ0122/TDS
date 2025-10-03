using DG.Tweening;
using UnityEngine;

public class DamageHandler : MonoBehaviour, IDamageHandler
{
    readonly int _hitEffectHash = Shader.PropertyToID("_FlashAmount");
    const float HIT_EFFECT_DURATION = 0.1f;

    IHealth _health;
    IHealth IDamageHandler.Health { get => _health; }

    SpriteRenderer[] _spriteRenderers;


    void Awake()
    {
        _health = GetComponent<IHealth>();
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        _health?.TakeDamage(damage);
        DamageFontManager.Instance.CreateDamageFont(damage, gameObject.transform);
        PlayHitEffet();
    }

    void PlayHitEffet()
    {
        foreach (var item in _spriteRenderers)
        {
            Material material = item.material;

            material.DOKill();
            material.DOFloat(1f, _hitEffectHash, HIT_EFFECT_DURATION / 2).SetLoops(2, LoopType.Yoyo);
        }
    }
}