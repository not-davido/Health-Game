using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseDetector : MonoBehaviour
{
    public bool IsGamePaused { get; private set; }

    private void Awake()
    {
        EventManager.AddListener<GamePauseEvent>(PauseGame);
    }

    void PauseGame(GamePauseEvent evt) {
        IsGamePaused = evt.paused;
    }
}
