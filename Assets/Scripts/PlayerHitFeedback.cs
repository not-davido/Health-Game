using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitFeedback : MonoBehaviour
{
    [Header("Damage")]
    [Tooltip("Color of the damage flash")]
    public Color DamageFlashColor;

    [Tooltip("Duration of the damage flash")]
    public float DamageFlashDuration;

    [Header("Heal")]
    [Tooltip("Color of the heal flash")]
    public Color HealFlashColor;

    [Tooltip("Duration of the heal flash")]
    public float HealFlashDuration;

    [SerializeField] private float delayBeforeTurningToOriginalColor = 0.5f;

    public bool m_FlashActive;
    float m_LastTimeFlashStarted = Mathf.NegativeInfinity;
    Health m_PlayerHealth;
    GameFlowManager m_GameFlowManager;
    SpriteRenderer SpriteRenderer;
    Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to player damage events
        PlayerController2D playerCharacterController = FindObjectOfType<PlayerController2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();

        m_PlayerHealth = playerCharacterController.GetComponent<Health>();

        m_GameFlowManager = FindObjectOfType<GameFlowManager>();

        m_PlayerHealth.OnDamaged += OnTakeDamage;
        m_PlayerHealth.OnHealed += OnHealed;

        originalColor = SpriteRenderer.material.GetColor("_Color");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_FlashActive) {
            float normalizedTimeSinceDamage = (Time.time - m_LastTimeFlashStarted) / DamageFlashDuration;

            if (normalizedTimeSinceDamage > 0f) {
                Color currentColor = SpriteRenderer.material.GetColor("_Color");
                SpriteRenderer.material.SetColor("_Color", Color.Lerp(currentColor, originalColor * 2.5f, normalizedTimeSinceDamage));
            } else if (normalizedTimeSinceDamage >= 1f) {
                m_FlashActive = false;
            }
        }
    }

    void ResetFlash() {
        m_LastTimeFlashStarted = Time.time + delayBeforeTurningToOriginalColor;
        m_FlashActive = true;
    }

    void OnTakeDamage(float dmg) {
        ResetFlash();
        SpriteRenderer.material.SetColor("_Color", DamageFlashColor * 10f);
        FindObjectOfType<MilkShake.Demo.ShakeButton>().UIShakeOnce();
    }

    void OnHealed(float amount) {
        ResetFlash();
        SpriteRenderer.material.SetColor("_Color", HealFlashColor * 10f);
    }
}
