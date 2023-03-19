using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(PlayerInputHandler))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private bool enableRun;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float smoothTimeSpeedChangeRate = 0.2f;
    [SerializeField] private bool smoothInput;
    [SerializeField] private float smoothTimeInput = 0.3f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 7f;
    [SerializeField] private float jumpCooldown = 0.2f;

    [Header("Ground")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundedDistance = 0.05f;
    [SerializeField] private float boxCastSizeAdjustmentX = 0;
    [SerializeField] private float boxCastSizeAdjustmentY = 0;
    [SerializeField] private bool debugGroundCheck;

    [Header("Prefabs")]
    [SerializeField] private GameObject formWhenDead;

    private Camera cam;
    private PlayerInputHandler inputHandler;
    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider2D;
    private Animator anim;
    private Health health;
    private float horizontalMovement;
    private float currentSpeed;
    private float jumpCooldownTimer;
    private bool jump;
    private float inputVelocity;
    private float speedChangeRateVelocity;
    private bool tookDamage;
    private bool diedFromFall;

    public bool isRunning { get; private set; }
    public bool isInvincible { get; private set; }
    public bool isDead { get; private set; }

    // Start is called before the first frame update
    void Start() {
        inputHandler = GetComponent<PlayerInputHandler>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();

        health.OnDie += OnDie;

        cam = Camera.main;

        rb2D.freezeRotation = true;
    }

    // Update is called once per frame
    void Update() {
        Movement();
        Jump();
        FlipSprite();
        UpdateAnimations();
        CheckIfBelowCamera();
    }

    private void FixedUpdate() {
        Vector2 velocity = rb2D.velocity;
        velocity.x = horizontalMovement * currentSpeed;
        rb2D.velocity = velocity;


        //var position = rb2D.position;
        //position.x += horizontalMovement * currentSpeed * Time.fixedDeltaTime;
        //rb2D.position = position;

        if (jump) {
            rb2D.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
            jump = false;
        }

        if (tookDamage) {
            rb2D.AddForce(new(0, 3f), ForceMode2D.Impulse);
            tookDamage = false;
        }
    }

    void FlipSprite() {
        if (horizontalMovement > 0.1f) {
            transform.rotation = Quaternion.identity;
        } else if (horizontalMovement < -0.1f) {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    public bool IsGrounded() {
        return Physics2D.BoxCast(boxCollider2D.bounds.center,
            boxCollider2D.bounds.size - new Vector3(boxCastSizeAdjustmentX, boxCastSizeAdjustmentY),
            0f, Vector2.down, groundedDistance, groundLayer);
    }

    void Movement() {
        // Check if we're holding the run button
        isRunning = inputHandler.run;

        horizontalMovement = smoothInput ?
            Mathf.SmoothDamp(horizontalMovement, inputHandler.move.x, ref inputVelocity, smoothTimeInput) :
            inputHandler.move.x;

        float targetSpeed = enableRun && isRunning ? runSpeed : walkSpeed;

        if (Mathf.Abs(horizontalMovement) < 0.1f) targetSpeed = 0;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedChangeRateVelocity, smoothTimeSpeedChangeRate);
    }

    void Jump() {
        if (IsGrounded()) {
            if (inputHandler.jump && Time.time > jumpCooldownTimer) {
                jump = true;
            }
        } else {
            jumpCooldownTimer = Time.time + jumpCooldown;
            inputHandler.jump = false;
        }
    }

    void UpdateAnimations() {
        if (anim != null) {
            anim.SetFloat("speed", Mathf.Abs(inputHandler.move.x));
            anim.SetFloat("y", rb2D.velocity.y);
            anim.SetBool("isGrounded", IsGrounded());
        }
    }

    public void Hit() {
        anim.SetTrigger("hit");
    }

    //private void OnDrawGizmos() {
    //    if (!UnityEditor.EditorApplication.isPlaying) return;

    //    Gizmos.color = IsGrounded() ? Color.green : Color.red;

    //    Gizmos.DrawWireCube(boxCollider2D.bounds.center - new Vector3(0f, groundedDistance),
    //            boxCollider2D.bounds.size - new Vector3(boxCastSizeAdjustmentX, boxCastSizeAdjustmentY));
    //}

    //private void OnBecameInvisible()
    //{
    //    if (transform.position.y < cameraTransform.position.y) {
    //        EventManager.Broadcast(Events.PlayerFailedEvent);
    //    }
    //}

    void CheckIfBelowCamera() {
        if (!isDead) {
            Vector3 position = cam.ViewportToWorldPoint(new Vector3(0.5f, 0));
            if (transform.position.y + 0.5f < position.y) {
                health.Kill();
            }
        }
    }

    void OnDie() {
        isDead = true;

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>().enabled = false;

        var g = Instantiate(formWhenDead, transform.position, transform.rotation);
        g.AddComponent<Rigidbody2D>().AddExplosionForce(500, 1.5f, diedFromFall ? ExplosionDirection.Up : ExplosionDirection.Center);

        EventManager.Broadcast(Events.PlayerFailedEvent);
    }
}
