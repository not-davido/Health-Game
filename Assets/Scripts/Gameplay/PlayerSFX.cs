using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip hitSFX;
    [SerializeField] private AudioClip healthPickupSFX;
    [SerializeField] private AudioClip deathSFX;

    private PlayerController2D player;
    private PlayerInputHandler inputHandler;
    private AudioSource audioSource;
    private Health health;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController2D>();
        inputHandler = GetComponent<PlayerInputHandler>();
        audioSource = GetComponent<AudioSource>();
        health = GetComponent<Health>();

        health.OnDamaged += OnDamage;
        health.OnHealed += OnHeal;
        health.OnDie += OnDie;
    }

    private void Update()
    {
        if (player.IsGrounded() && inputHandler.jump) {
            audioSource.PlayOneShot(jumpSFX);
        }
    }

    void OnDamage(float value) {
        audioSource.PlayOneShot(hitSFX);
    }

    void OnHeal(float value) {
        audioSource.PlayOneShot(healthPickupSFX);
    }

    void OnDie() {
        //AudioSource.PlayClipAtPoint(deathSFX, transform.position);
        audioSource.PlayOneShot(deathSFX);
    }

    private void OnDisable()
    {
        health.OnDamaged -= OnDamage;
        health.OnHealed -= OnHeal;
        health.OnDie -= OnDie;
    }
}
