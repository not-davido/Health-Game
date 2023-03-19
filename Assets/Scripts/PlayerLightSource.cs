using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLightSource : MonoBehaviour
{
    [SerializeField] private Light2D playerLight;

    [SerializeField] private float outerLightRadiusEasyMode = 5;
    [SerializeField] private float outerLightRadiusNormalMode = 3.5f;
    [SerializeField] private float outerLightRadiusHardMode = 1.8f;

    // Start is called before the first frame update
    void Awake()
    {
        GameDifficulty difficulty = FindObjectOfType<GameDifficulty>();

        if (difficulty != null) {
            switch (difficulty.mode) {
                case GameDifficulty.Mode.Easy:
                    playerLight.pointLightOuterRadius = outerLightRadiusEasyMode;
                    break;
                case GameDifficulty.Mode.Normal:
                    playerLight.pointLightOuterRadius = outerLightRadiusNormalMode;
                    break;
                case GameDifficulty.Mode.Impossible:
                    playerLight.pointLightOuterRadius = outerLightRadiusHardMode;
                    break;
            }
        } else {
            playerLight.pointLightOuterRadius = 4.05f;
        }
    }
}
