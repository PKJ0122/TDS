using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PoolObject))]
public class HpBar : MonoBehaviour
{
    Canvas _canvas;
    Slider _slider;
    IHealth _health;
    PoolObject _poolObject;

    Action RelasePoolHandler;


    void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _slider = transform.Find("Slider").GetComponent<Slider>();
        _poolObject = GetComponent<PoolObject>();
        RelasePoolHandler += () =>
        {
            _poolObject.RelasePool();
        };
    }

    public void SetHpBar(IHealth health)
    {
        _health = health;
        _slider.maxValue = _health.MaxHp;
        _slider.value = _health.MaxHp;
        _canvas.enabled = false;
        _health.OnHealthChange += Refresh;
        _health.OnDie += RelasePoolHandler;
    }

    void OnDisable()
    {
        _health.OnHealthChange -= Refresh;
        _health.OnDie -= RelasePoolHandler;
        _health = null;
    }

    void Refresh(float value)
    {
        _slider.value = value;
        _canvas.enabled = _health.MaxHp > value;
    }
}
