using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject parent;

    private List<GameObject> bars = new();
    // Start is called before the first frame update
    void Start()
    {
        PlayerController2D player = FindObjectOfType<PlayerController2D>();
        Health health = player.GetComponent<Health>();

        health.OnDamaged += OnDamage;
        health.OnHealed += OnHeal;

        int barCount = parent.transform.childCount - health.CurrentHealth;
        int i = 0;

        foreach (Transform child in parent.transform) {
            if (i == barCount) break;
            child.gameObject.SetActive(false);
            i++;
        }

        foreach (Transform child in parent.transform) {
            if (child.gameObject.activeInHierarchy) {
                bars.Add(child.gameObject);
            }
        }
    }

    void OnDamage(float amount) {
        int i = 0;
        foreach (var b in bars.Where(b => b.activeInHierarchy)) {
            if (i == (int)amount) break;
            b.SetActive(false);
            i++;
        }
    }

    void OnHeal(float amount) {
        int i = 0;
        foreach (var b in bars.Where(b => !b.activeInHierarchy)) {
            if (i == (int)amount) break;
            b.SetActive(true);
            i++;
        }
    }
}
