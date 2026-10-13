using UnityEngine;

public class CoinController : MonoBehaviour
{
    private CoinFactory coinFactory;
    private bool collected;

    private void OnEnable()
    {
        collected = false;
    }

    public void Initialize(CoinFactory factory)
    {
        coinFactory = factory;
        collected = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collected || !collision.gameObject.CompareTag("Player"))
            return;

        PlayerController player = collision.GetComponent<PlayerController>();

        if (player == null)
            player = collision.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        collected = true;
        player.PickUpCoin();

        if (coinFactory != null)
            coinFactory.Recycle(this);
        else
            gameObject.SetActive(false);
    }
}