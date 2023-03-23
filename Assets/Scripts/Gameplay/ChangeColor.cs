using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChangeColor : MonoBehaviour
{
    [SerializeField] private float intensity = 5f;
    [SerializeField] private Light2D lightSource;

    SpriteRenderer SpriteRenderer;
    Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = SpriteRenderer.material.GetColor("_Color");
    }

    public void Green() {
        SpriteRenderer.material.SetColor("_Color", Color.green * intensity);
        lightSource.color = Color.green;
    }

    public void Yellow() {
        SpriteRenderer.material.SetColor("_Color", Color.yellow * intensity);
        lightSource.color = Color.yellow;
    }

    public void Red() {
        SpriteRenderer.material.SetColor("_Color", Color.red * intensity);
        lightSource.color = Color.red;
    }

    public void White() {
        SpriteRenderer.material.SetColor("_Color", originalColor * intensity);
        lightSource.color = Color.white;
    }
}
