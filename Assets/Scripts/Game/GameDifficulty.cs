using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDifficulty : MonoBehaviour
{
    public enum Mode {
        Easy, Normal, Hard, Custom
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
                case Mode.Custom:
                default:
                    return "white";
            }
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        EventManager.AddListener<GameQuitEvent>(OnQuit);
        EventManager.AddListener<GameCompletedEvent>(GameEnded);
    }

    void OnQuit(GameQuitEvent evt) {
        Destroy(gameObject);
    }

    void GameEnded(GameCompletedEvent evt) {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<GameQuitEvent>(OnQuit);
        EventManager.RemoveListener<GameCompletedEvent>(GameEnded);
    }
}
