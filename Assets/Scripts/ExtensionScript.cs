using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionScript
{
    public static Collider2D[] AddExplosionForce(this Rigidbody2D rb2d, float force, float radius) {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(rb2d.position, radius);

        foreach (var collider in collisions) {
            if (collider.TryGetComponent(out Rigidbody2D rb)) {
                var dir = collider.transform.position - rb2d.transform.position;
                var dist = dir.magnitude;

                if (dist > 0) {
                    if (dist < 1)
                        dist = 1;

                    // Calculate explosion force depending on distance; closer is stronger while far is weaker force
                    float explosionForce = force / dist;

                    rb.AddForce(dir.normalized * explosionForce);
                }
            }
        }

        return collisions;
    }
}
