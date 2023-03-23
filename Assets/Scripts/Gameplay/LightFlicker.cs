using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    public enum FlickerMode {
        Random,
        AnimationCurve
    }

    public Light2D flickeringLight;
    public Renderer flickeringRenderer;
    public FlickerMode flickerMode;
    public float lightIntensityMin = 1.25f;
    public float lightIntensityMax = 2.25f;
    public float flickerDuration = 0.075f;
    public AnimationCurve intensityCurve;

    Material m_FlickeringMaterial;
    Color m_EmissionColor;
    float m_Timer;
    float m_FlickerLightIntensity;

    //static readonly int k_EmissionColorID = Shader.PropertyToID(k_EmissiveColorName);

    const string k_EmissiveColorName = "_Color";
    //const string k_EmissionName = "_Emission";
    const float k_LightIntensityToEmission = 2f / 3f;

    void Start() {
        if (flickeringRenderer != null) {
            m_FlickeringMaterial = flickeringRenderer.material;
            m_EmissionColor = m_FlickeringMaterial.GetColor(k_EmissiveColorName);
        }
    }

    void Update() {
        m_Timer += Time.deltaTime;

        if (flickerMode == FlickerMode.Random) {
            if (m_Timer >= flickerDuration) {
                ChangeRandomFlickerLightIntensity();
            }
        } else if (flickerMode == FlickerMode.AnimationCurve) {
            ChangeAnimatedFlickerLightIntensity();
        }

        float intensity = Mathf.Lerp(flickeringLight.intensity, m_FlickerLightIntensity, 10 * Time.deltaTime);
        flickeringLight.intensity = intensity;
        if (m_FlickeringMaterial != null)
            m_FlickeringMaterial.SetColor(k_EmissiveColorName, m_EmissionColor * intensity * k_LightIntensityToEmission);
    }

    void ChangeRandomFlickerLightIntensity() {
        m_FlickerLightIntensity = Random.Range(lightIntensityMin, lightIntensityMax);

        m_Timer = 0f;
    }

    void ChangeAnimatedFlickerLightIntensity() {
        m_FlickerLightIntensity = intensityCurve.Evaluate(m_Timer);

        if (m_Timer >= intensityCurve[intensityCurve.length - 1].time)
            m_Timer = intensityCurve[0].time;
    }
}
