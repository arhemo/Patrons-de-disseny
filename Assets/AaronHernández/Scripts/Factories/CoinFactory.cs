using UnityEngine;

public class CoinFactory : MonoBehaviour
{
    [SerializeField] private ObjectPool coinPool;

    public CoinController Create(Vector3 position)
    {
        if (coinPool == null)
            return null;

        GameObject instance = coinPool.Get(position, Quaternion.identity);

        if (instance == null)
            return null;

        CoinController coin = instance.GetComponent<CoinController>();

        if (coin == null)
        {
            coinPool.Return(instance);
            return null;
        }

        coin.Initialize(this);
        return coin;
    }

    public void Recycle(CoinController coin)
    {
        if (coin != null && coinPool != null)
            coinPool.Return(coin.gameObject);
    }
}