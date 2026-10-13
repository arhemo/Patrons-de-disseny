using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [SerializeField] private EnemyFactory enemyFactory;
    [SerializeField] private Transform[] spawnPoints;

    private readonly HashSet<GameObject> activeEnemies = new HashSet<GameObject>();
    private bool isSpawningWave;
    private bool hasStarted;

    public int ActiveEnemyCount => activeEnemies.Count;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetInstance()
    {
        Instance = null;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        if (Instance != this)
            return;

        GameEvents.OnEnemySpawned += RegisterSpawn;
        GameEvents.OnEnemyDiedInstance += RegisterDeath;

        if (hasStarted)
        {
            activeEnemies.Clear();
            RegisterExistingEnemies();

            if (activeEnemies.Count == 0)
                SpawnWave();
        }
    }

    private void OnDisable()
    {
        GameEvents.OnEnemySpawned -= RegisterSpawn;
        GameEvents.OnEnemyDiedInstance -= RegisterDeath;
    }

    private void Start()
    {
        hasStarted = true;
        RegisterExistingEnemies();

        if (activeEnemies.Count == 0)
            SpawnWave();
    }

    private void RegisterExistingEnemies()
    {
        HealthController[] enemies = FindObjectsByType<HealthController>(FindObjectsSortMode.None);

        foreach (HealthController enemy in enemies)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy)
                continue;

            if (!enemy.gameObject.CompareTag("Enemy"))
                continue;

            if (enemyFactory != null)
                enemy.SetFactory(enemyFactory);

            activeEnemies.Add(enemy.gameObject);
        }
    }

    private void RegisterSpawn(GameObject enemy)
    {
        if (enemy != null)
            activeEnemies.Add(enemy);
    }

    private void RegisterDeath(GameObject enemy)
    {
        if (enemy == null || !activeEnemies.Remove(enemy))
            return;

        if (hasStarted && !isSpawningWave && activeEnemies.Count == 0)
            SpawnWave();
    }

    public void SpawnWave()
    {
        if (isSpawningWave || activeEnemies.Count > 0 || enemyFactory == null)
            return;

        if (spawnPoints == null || spawnPoints.Length == 0)
            return;

        isSpawningWave = true;

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint == null)
                continue;

            GameObject enemy = enemyFactory.Create(spawnPoint.position);

            if (enemy != null && enemy.activeInHierarchy)
                activeEnemies.Add(enemy);
        }

        isSpawningWave = false;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
