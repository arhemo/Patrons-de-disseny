using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<int> OnCoinsChanged;
    public static event Action OnCoinCollected;
    public static event Action<Vector3> OnEnemyDied;
    public static event Action<GameObject> OnEnemySpawned;
    public static event Action<GameObject> OnEnemyDiedInstance;
    public static event Action OnPlayerJumped;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetEvents()
    {
        OnCoinsChanged = null;
        OnCoinCollected = null;
        OnEnemyDied = null;
        OnEnemySpawned = null;
        OnEnemyDiedInstance = null;
        OnPlayerJumped = null;
    }

    public static void CoinsChanged(int totalCoins)
    {
        OnCoinsChanged?.Invoke(totalCoins);
    }

    public static void CoinCollected()
    {
        OnCoinCollected?.Invoke();
    }

    public static void EnemyDied(Vector3 position)
    {
        OnEnemyDied?.Invoke(position);
    }

    public static void EnemySpawned(GameObject enemy)
    {
        OnEnemySpawned?.Invoke(enemy);
    }

    public static void EnemyDiedInstance(GameObject enemy)
    {
        OnEnemyDiedInstance?.Invoke(enemy);
    }

    public static void PlayerJumped()
    {
        OnPlayerJumped?.Invoke();
    }
}