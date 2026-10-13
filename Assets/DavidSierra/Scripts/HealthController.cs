using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {
    [SerializeField] int maxLives = 3;
    [SerializeField] int lives = 3;
    public void GetHit() {
        lives--;
        if (lives <= 0) {
            Destroy(gameObject);
        }
    }
}
