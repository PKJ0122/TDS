using UnityEngine;

[RequireComponent(typeof(PoolObject))]
public class Bullet : MonoBehaviour
{
    [Tooltip("ÃÑ¾Ë ¼Óµµ")]
    public float moveSpeed = 4f;

    Rigidbody2D _rb;
    PoolObject _pool;

    bool _isActive;
    float _atk;
    Transform _traget;
    Vector3 _direction;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _pool = GetComponent<PoolObject>();
    }

    void FixedUpdate()
    {
        if (!_isActive) return;

        _direction = AdjustDirection();
        _rb.velocity = _direction.normalized * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && _isActive)
        {
            _isActive = false;
            _pool.RelasePool();

            if (collision.gameObject.TryGetComponent(out IDamageHandler damageHandler))
            {
                damageHandler.TakeDamage(_atk);
            }
        }
        else if (collision.gameObject.CompareTag("Ground") && _isActive)
        {
            _isActive = false;
            _pool.RelasePool();
        }
    }

    public void Shot(float atk, IDamageHandler damageHandler)
    {
        _atk = atk;
        _isActive = true;
        _direction = SetDirection(damageHandler);
    }

    Vector3 SetDirection(IDamageHandler damageHandler)
    {
        Vector3 target = BattleManager.Instance.gameObject.transform.position;

        if (damageHandler != null)
        {
            Component component = damageHandler as Component;
            if (component != null)
            {
                target = component.transform.position;
                _traget = component.transform;
            }
        }

        Vector3 direction = target - transform.position;
        return direction;
    }

    Vector3 AdjustDirection()
    {
        if (_traget == null || !_traget.gameObject.activeSelf) return _direction;

        Vector3 direction = _traget.position - transform.position;
        return direction;
    }
}