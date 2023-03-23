using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    public ObjectPool<GameObject> pool;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Health playerHealth)) {
            playerHealth.TakeDamage(1);
        }

        pool.Release(gameObject);
    }
}
