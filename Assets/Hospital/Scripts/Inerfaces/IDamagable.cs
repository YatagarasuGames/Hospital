using Mirror;

public interface IDamagable
{
    [Server]
    public void GetDamage(float damage);
}
