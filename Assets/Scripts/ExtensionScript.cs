using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionScript
{
    public static Collider2D[] AddExplosionForce(this Rigidbody2D rb2d, float force, float radius, ExplosionDirection direction = ExplosionDirection.Center) {
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

                    Vector2 explosionDirection = Vector2.one;

                    switch (direction) {
                        case ExplosionDirection.Center:
                            explosionDirection = Vector2.one;
                            break;
                        case ExplosionDirection.Up:
                            explosionDirection = Vector2.up * 50;
                            break;
                    }

                    rb.AddForce(dir.normalized * explosionForce * explosionDirection);
                }
            }
        }

        return collisions;
    }
}

public enum ExplosionDirection {
    Center,
    Up,
}
