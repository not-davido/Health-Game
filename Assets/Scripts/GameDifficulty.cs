using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDifficulty : MonoBehaviour
{
    public enum Mode {
        Easy, Normal, Hard
    }

    public Mode mode;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
