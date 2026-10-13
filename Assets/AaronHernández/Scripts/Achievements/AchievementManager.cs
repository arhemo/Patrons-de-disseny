using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    [SerializeField] private NotificationManager notificationManager;

    private int enemiesKilled;
    private int coinsCollected;
    private int jumpsPerformed;

    private bool enemiesAchievement;
    private bool coinsAchievement;
    private bool jumpsAchievement;

    public bool EnemiesAchievementUnlocked => enemiesAchievement;
    public bool CoinsAchievementUnlocked => coinsAchievement;
    public bool JumpsAchievementUnlocked => jumpsAchievement;

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

        GameEvents.OnEnemyDied += RegisterEnemyKill;
        GameEvents.OnCoinCollected += RegisterCoinCollected;
        GameEvents.OnPlayerJumped += RegisterJump;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyDied -= RegisterEnemyKill;
        GameEvents.OnCoinCollected -= RegisterCoinCollected;
        GameEvents.OnPlayerJumped -= RegisterJump;
    }

    private void RegisterEnemyKill(Vector3 position)
    {
        enemiesKilled++;

        if (!enemiesAchievement && enemiesKilled >= 3)
        {
            enemiesAchievement = true;
            Notify("Logro desbloqueado: elimina 3 enemigos");
        }
    }

    private void RegisterCoinCollected()
    {
        coinsCollected++;

        if (!coinsAchievement && coinsCollected >= 5)
        {
            coinsAchievement = true;
            Notify("Logro desbloqueado: recoge 5 monedas");
        }
    }

    private void RegisterJump()
    {
        jumpsPerformed++;

        if (!jumpsAchievement && jumpsPerformed >= 10)
        {
            jumpsAchievement = true;
            Notify("Logro desbloqueado: salta 10 veces");
        }
    }

    private void Notify(string message)
    {
        if (notificationManager != null)
            notificationManager.AddNotification(message, 2f);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}