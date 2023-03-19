using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDifficulty : MonoBehaviour
{
    public enum Mode {
        Easy, Normal, Hard
    }

    public Mode mode;

    public string textColor {
        get {
            switch (mode) {
                case Mode.Easy:
                    return "green";
                case Mode.Normal:
                    return "yellow";
                case Mode.Hard:
                    return "red";
                default:
                    return "white";
            }
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
