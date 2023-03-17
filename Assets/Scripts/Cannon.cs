using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileForce = 10f;
    [SerializeField] private Transform cannonTip;
    [SerializeField] private float projectileInterval = 0.5f;
    [SerializeField] private float distanceToFire = 3;
    [SerializeField] private float smoothTime = 0.3f;

    private Transform player;
    private float interval;
    private Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController2D>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        var dir = (player.position - transform.position).normalized;
        var dist = (player.position - transform.position).sqrMagnitude;

        if (dist <= distanceToFire * distanceToFire) {
            transform.right = Vector2.SmoothDamp(transform.right, dir, ref velocity, smoothTime);

            if (Time.time > interval) {
                GameObject p = Instantiate(projectile, cannonTip.position, cannonTip.rotation);
                Rigidbody2D rb = p.AddComponent<Rigidbody2D>();
                rb.velocity = cannonTip.right * projectileForce;

                SpriteRenderer renderer = p.GetComponent<SpriteRenderer>();
                renderer.color = new Color(Random.value, Random.value, Random.value, 1f);

                interval = Time.time + projectileInterval;
            }
        }
    }
}
