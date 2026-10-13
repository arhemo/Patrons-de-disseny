using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameRunning { get; private set; } = true;
    public int Coins { get; private set; }

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

    public void SetGameRunning(bool isRunning)
    {
        IsGameRunning = isRunning;
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0)
            return;

        Coins += amount;
        GameEvents.CoinsChanged(Coins);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
