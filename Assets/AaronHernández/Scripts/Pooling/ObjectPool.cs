using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField, Min(0)] private int initialSize = 5;

    private readonly Queue<GameObject> available = new Queue<GameObject>();
    private readonly HashSet<GameObject> availableSet = new HashSet<GameObject>();

    private void Awake()
    {
        if (prefab == null)
            return;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject instance = Instantiate(prefab, transform);
            instance.SetActive(false);
            available.Enqueue(instance);
            availableSet.Add(instance);
        }
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject instance = null;

        while (available.Count > 0 && instance == null)
        {
            instance = available.Dequeue();

            if (instance != null)
                availableSet.Remove(instance);
        }

        if (instance == null)
        {
            if (prefab == null)
                return null;

            instance = Instantiate(prefab, transform);
        }

        instance.transform.SetParent(transform);
        instance.transform.SetPositionAndRotation(position, rotation);

        if (!instance.activeSelf)
            instance.SetActive(true);

        return instance;
    }

    public void Return(GameObject instance)
    {
        if (instance == null || availableSet.Contains(instance))
            return;

        instance.SetActive(false);
        instance.transform.SetParent(transform);
        available.Enqueue(instance);
        availableSet.Add(instance);
    }
}