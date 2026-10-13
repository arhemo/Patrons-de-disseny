using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private ObjectPool enemyPool;

    public GameObject Create(Vector3 position)
    {
        if (enemyPool == null)
            return null;

        GameObject enemy = enemyPool.Get(position, Quaternion.identity);

        if (enemy == null)
            return null;

        HealthController healthController = enemy.GetComponent<HealthController>();

        if (healthController == null)
        {
            enemyPool.Return(enemy);
            return null;
        }

        enemy.transform.SetPositionAndRotation(position, Quaternion.identity);
        healthController.Initialize(this);
        GameEvents.EnemySpawned(enemy);

        return enemy;
    }

    public void Recycle(GameObject enemy)
    {
        if (enemy == null)
            return;

        if (enemyPool != null)
            enemyPool.Return(enemy);
        else
            enemy.SetActive(false);
    }
}