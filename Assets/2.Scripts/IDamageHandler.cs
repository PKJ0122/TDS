public interface IDamageHandler
{
    IHealth Health { get; }

    void TakeDamage(float damage);
}
