using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomModeHealthPointSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMPro.TextMeshProUGUI textValue;

    public float VALUE { get; private set; }

    private void OnEnable()
    {
        slider.onValueChanged.AddListener(UpdateSlider);
        UpdateSlider(slider.value);
    }

    void UpdateSlider(float value) {
        VALUE = value;
        textValue.text = slider.value.ToString();
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(UpdateSlider);
    }
}
