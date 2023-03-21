using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Maximum amount of health")]
    [SerializeField] private int maxHealthEasyMode = 10;
    [SerializeField] private int maxHealthNormalMode = 5;
    [SerializeField] private int maxHealthHardMode = 3;

    [Tooltip("Health ratio at which the critical health vignette starts appearing")]
    [SerializeField] private float criticalHealthRatio = 0.3f;

    public Action<float> OnDamaged;
    public Action<float> OnHealed;
    public Action OnDie;

    public int CurrentHealth { get; set; }
    public bool Invincible { get; set; }
    public bool CanPickup() => CurrentHealth < maxHealth;

    public float GetRatio() => CurrentHealth / maxHealth;
    public bool IsCritical() => GetRatio() <= criticalHealthRatio;

    bool m_IsDead;
    int maxHealth;

    public float MaxHealth => maxHealth;
    public float CriticalHealthRatio => criticalHealthRatio;

    void Awake() {
        GameDifficulty difficulty = FindObjectOfType<GameDifficulty>();

        if (difficulty != null) {
            switch (difficulty.mode) {
                case GameDifficulty.Mode.Easy:
                    maxHealth = maxHealthEasyMode;
                    break;
                case GameDifficulty.Mode.Normal:
                    maxHealth = maxHealthNormalMode;
                    break;
                case GameDifficulty.Mode.Hard:
                    maxHealth = maxHealthHardMode;
                    break;
                case GameDifficulty.Mode.Custom:
                    maxHealth = FindObjectOfType<CustomMode>().HealthPoints;
                    break;
            }
        } else {
            maxHealth = 10;
        }

        CurrentHealth = maxHealth;
    }

    public void Heal(int healAmount) {
        float healthBefore = CurrentHealth;
        CurrentHealth += healAmount;
        CurrentHealth = (int)Mathf.Clamp(CurrentHealth, 0f, maxHealth);

        // call OnHeal action
        float trueHealAmount = CurrentHealth - healthBefore;
        if (trueHealAmount > 0f) {
            OnHealed?.Invoke(trueHealAmount);
        }
    }

    public void TakeDamage(int damage) {
        if (Invincible)
            return;

        float healthBefore = CurrentHealth;
        CurrentHealth -= damage;
        CurrentHealth = (int)Mathf.Clamp(CurrentHealth, 0f, maxHealth);

        // call OnDamage action
        float trueDamageAmount = healthBefore - CurrentHealth;
        if (trueDamageAmount > 0f) {
            OnDamaged?.Invoke(trueDamageAmount);
        }

        HandleDeath();
    }

    public void Kill() {
        CurrentHealth = 0;

        // call OnDamage action
        OnDamaged?.Invoke(maxHealth);

        HandleDeath();
    }

    void HandleDeath() {
        if (m_IsDead)
            return;

        // call OnDie action
        if (CurrentHealth <= 0f) {
            m_IsDead = true;
            OnDie?.Invoke();
        }
    }
}

