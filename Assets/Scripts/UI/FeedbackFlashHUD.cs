using UnityEngine;
using UnityEngine.UI;

public class FeedbackFlashHUD : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Image component of the flash")]
    public Image FlashImage;

    [Tooltip("CanvasGroup to fade the damage flash, used when recieving damage end healing")]
    public CanvasGroup FlashCanvasGroup;

    [Tooltip("CanvasGroup to fade the critical health vignette")]
    public CanvasGroup VignetteCanvasGroup;

    [Header("Damage")]
    [Tooltip("Color of the damage flash")]
    public Color DamageFlashColor;

    [Tooltip("Duration of the damage flash")]
    public float DamageFlashDuration;

    [Tooltip("Max alpha of the damage flash")]
    public float DamageFlashMaxAlpha = 1f;

    [Header("Critical health")]
    [Tooltip("Max alpha of the critical vignette")]
    public float CriticaHealthVignetteMaxAlpha = .8f;

    [Tooltip("Frequency at which the vignette will pulse when at critical health")]
    public float PulsatingVignetteFrequency = 4f;

    [Header("Heal")]
    [Tooltip("Color of the heal flash")]
    public Color HealFlashColor;

    [Tooltip("Duration of the heal flash")]
    public float HealFlashDuration;

    [Tooltip("Max alpha of the heal flash")]
    public float HealFlashMaxAlpha = 1f;

    bool m_FlashActive;
    Health m_PlayerHealth;
    GameFlowManager m_GameFlowManager;

    void Start() {
        PlayerController2D playerCharacterController = FindObjectOfType<PlayerController2D>();

        m_PlayerHealth = playerCharacterController.GetComponent<Health>();

        m_GameFlowManager = FindObjectOfType<GameFlowManager>();
    }

    void Update() {
        if (m_PlayerHealth.IsCritical()) {
            VignetteCanvasGroup.gameObject.SetActive(true);
            float vignetteAlpha =
                (1 - (m_PlayerHealth.CurrentHealth / m_PlayerHealth.MaxHealth /
                      m_PlayerHealth.CriticalHealthRatio)) * CriticaHealthVignetteMaxAlpha;

            if (m_GameFlowManager.GameIsEnding)
                VignetteCanvasGroup.alpha = vignetteAlpha;
            else
                VignetteCanvasGroup.alpha =
                    ((Mathf.Sin(Time.time * PulsatingVignetteFrequency) / 2) + 0.5f) * vignetteAlpha;
        } else {
            VignetteCanvasGroup.gameObject.SetActive(false);
        }
    }
}
