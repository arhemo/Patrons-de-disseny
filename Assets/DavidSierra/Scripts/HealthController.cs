using UnityEngine;

public class HealthController : MonoBehaviour, IDamageable
{
    [SerializeField, Min(1)] private int maxLives = 3;
    [SerializeField, Min(1)] private int lives = 3;

    private bool isDead;
    private EnemyFactory enemyFactory;

    private void OnEnable()
    {
        isDead = false;
        lives = Mathf.Max(1, maxLives);
    }

    public void Initialize(EnemyFactory factory)
    {
        enemyFactory = factory;
        isDead = false;
        lives = Mathf.Max(1, maxLives);
    }

    public void SetFactory(EnemyFactory factory)
    {
        enemyFactory = factory;
    }

    public void GetHit()
    {
        if (isDead)
            return;

        lives--;

        if (lives <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;
        Vector3 deathPosition = transform.position;
        GameObject deadEnemy = gameObject;

        GameEvents.EnemyDied(deathPosition);

        if (enemyFactory != null)
            enemyFactory.Recycle(deadEnemy);
        else
            deadEnemy.SetActive(false);

        GameEvents.EnemyDiedInstance(deadEnemy);
    }
}