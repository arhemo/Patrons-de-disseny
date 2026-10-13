using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private float projectileSpeed = 10f;

    private ProjectileFactory projectileFactory;
    private bool hasCollided;

    private void Awake()
    {
        if (rb2D == null)
            rb2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        hasCollided = false;
    }

    public void Init(Vector2 direction)
    {
        if (rb2D == null)
            return;

        rb2D.linearVelocity = direction.normalized * projectileSpeed;
    }

    public void SetFactory(ProjectileFactory factory)
    {
        projectileFactory = factory;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCollided)
            return;

        hasCollided = true;

        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            damageable.GetHit();

        if (projectileFactory != null)
            projectileFactory.Recycle(this);
        else
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (rb2D != null)
            rb2D.linearVelocity = Vector2.zero;
    }
}