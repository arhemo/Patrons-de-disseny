using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] Rigidbody2D rb2D;
    [SerializeField] float projectileSpeed;
    public void Init(Vector2 direction) {
        rb2D.linearVelocity = direction * projectileSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.CompareTag("Enemy")) {
            collision.gameObject.GetComponent<HealthController>().GetHit();
        }
        Destroy(gameObject);
    }

}
