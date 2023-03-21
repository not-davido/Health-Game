using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attempts : MonoBehaviour
{
    public int attempt { get; private set; } = 1;

    private static Attempts s_Instance;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (s_Instance == null)
            s_Instance = this;
        else
            Destroy(gameObject);

        EventManager.AddListener<PlayerFailedEvent>(OnPLayerFailed);
        EventManager.AddListener<GameQuitEvent>(OnQuit);
        EventManager.AddListener<GameCompletedEvent>(GameEnded);
    }

    void OnPLayerFailed(PlayerFailedEvent evt) {
        attempt++;
    }

    void OnQuit(GameQuitEvent evt) {
        Destroy(gameObject);
    }

    void GameEnded(GameCompletedEvent evt) {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<PlayerFailedEvent>(OnPLayerFailed);
        EventManager.RemoveListener<GameQuitEvent>(OnQuit);
        EventManager.RemoveListener<GameCompletedEvent>(GameEnded);
    }
}
