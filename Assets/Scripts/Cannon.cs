using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private int maxSize = 10;
    [SerializeField] private float projectileForce = 10f;
    [SerializeField] private Transform cannonTip;
    [SerializeField] private float projectileInterval = 0.5f;
    [SerializeField] private float distanceToFire = 3;
    [SerializeField] private float smoothTime = 0.3f;

    private PlayerController2D player;
    private float interval;
    private Vector2 velocity;
    private bool canFire;

    private ObjectPool<GameObject> projectilePool;

    private void Awake()
    {
        StartGame.OnGameStarted += CanFire;
        EventManager.AddListener<GameCompletedEvent>(GameEnded);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController2D>();

        projectilePool = new(OnCreate, OnGet, OnRelease, OnDestroyPool, false, 10, maxSize);
    }

    // Update is called once per frame
    void Update()
    {
        var dir = (player.transform.position - transform.position).normalized;
        var dist = (player.transform.position - transform.position).sqrMagnitude;

        if (dist <= distanceToFire * distanceToFire) {
            transform.right = Vector2.SmoothDamp(transform.right, dir, ref velocity, smoothTime);

            if (!player.isDead && canFire && Time.time > interval) {
                var p = projectilePool.Get();
                p.transform.SetPositionAndRotation(cannonTip.position, Quaternion.identity);

                p.GetComponent<Rigidbody2D>().velocity = cannonTip.right * projectileForce;
                p.GetComponent<Projectile>().pool = projectilePool;

                var randomColor = new Color(Random.value, Random.value, Random.value, 1);
                p.GetComponent<SpriteRenderer>().material.SetColor("_Color", randomColor * 5f);

                interval = Time.time + projectileInterval;
            }
        }
    }

    GameObject OnCreate() {
        GameObject p = Instantiate(projectile);
        p.AddComponent<Rigidbody2D>();
        p.AddComponent<Projectile>();
        return p;
    }

    void OnGet(GameObject gameObject) {
        gameObject.SetActive(true);
    }

    void OnRelease(GameObject gameObject) {
        gameObject.SetActive(false);
    }

    void OnDestroyPool(GameObject gameObject) {
        Destroy(gameObject);
    }

    void CanFire(bool value) {
        canFire = value;
    }

    void GameEnded(GameCompletedEvent evt) => CanFire(false);

    private void OnDisable()
    {
        StartGame.OnGameStarted -= CanFire;
        EventManager.RemoveListener<GameCompletedEvent>(GameEnded);
    }
}
