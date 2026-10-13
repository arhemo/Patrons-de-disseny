using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    [SerializeField] private ObjectPool projectilePool;

    public Projectile Create(Vector3 position, Vector2 direction)
    {
        if (projectilePool == null)
            return null;

        GameObject instance = projectilePool.Get(position, Quaternion.identity);

        if (instance == null)
            return null;

        Projectile projectile = instance.GetComponent<Projectile>();

        if (projectile == null)
        {
            projectilePool.Return(instance);
            return null;
        }

        projectile.SetFactory(this);
        projectile.Init(direction);
        return projectile;
    }

    public void Recycle(Projectile projectile)
    {
        if (projectile != null && projectilePool != null)
            projectilePool.Return(projectile.gameObject);
    }
}