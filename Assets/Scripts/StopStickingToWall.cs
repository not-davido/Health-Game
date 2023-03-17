using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopStickingToWall : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private float boxCastSizeAdjustmentX = 0;
    [SerializeField] private float boxCastSizeAdjustmentY = 0;
    [SerializeField] private bool isHittingObstacle;

    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isHittingObstacle = Physics2D.BoxCast(transform.position,
            boxCollider2D.bounds.size + new Vector3(boxCastSizeAdjustmentX, boxCastSizeAdjustmentY),
            0f, Vector2.zero, 0, obstacleLayers);
    }

    private void FixedUpdate()
    {
        if (isHittingObstacle) {
            var vel = rb2D.velocity;
            vel.x = 0;
            rb2D.velocity = vel;
        }
    }

    private void OnDrawGizmos() {
        if (!UnityEditor.EditorApplication.isPlaying) return;

        Gizmos.color = isHittingObstacle ? Color.red : Color.green;

        Gizmos.DrawWireCube(boxCollider2D.bounds.center,
                boxCollider2D.bounds.size + new Vector3(boxCastSizeAdjustmentX, boxCastSizeAdjustmentY));
    }
}
