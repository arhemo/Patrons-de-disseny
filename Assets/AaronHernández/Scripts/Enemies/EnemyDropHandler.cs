using UnityEngine;

public class EnemyDropHandler : MonoBehaviour
{
    [SerializeField] private CoinFactory coinFactory;

    private void OnEnable()
    {
        GameEvents.OnEnemyDied += HandleEnemyDied;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyDied -= HandleEnemyDied;
    }

    private void HandleEnemyDied(Vector3 deathPosition)
    {
        if (coinFactory != null)
            coinFactory.Create(deathPosition);
    }
}
