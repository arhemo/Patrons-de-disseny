using TMPro;
using UnityEngine;

public class CoinHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    private void OnEnable()
    {
        GameEvents.OnCoinsChanged += UpdateCoins;
        RefreshCoins();
    }

    private void Start()
    {
        RefreshCoins();
    }

    private void OnDisable()
    {
        GameEvents.OnCoinsChanged -= UpdateCoins;
    }

    private void RefreshCoins()
    {
        if (GameManager.Instance != null)
            UpdateCoins(GameManager.Instance.Coins);
    }

    private void UpdateCoins(int totalCoins)
    {
        if (coinText != null)
            coinText.text = totalCoins.ToString();
    }
}