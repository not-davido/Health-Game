using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private float startingUpFadeDelay = 0.5f;
    [SerializeField] private float fadeDelay = 1.5f;
    [SerializeField] private float delayBeforeStarting = 2f;

    private float fadeTimer;
    private bool menuStarting;
    private bool gameIsStarting;

    public void EasyMode() {
        FindObjectOfType<GameDifficulty>().mode = GameDifficulty.Mode.Easy;
        StartGame();
    }

    public void NormalMode() {
        FindObjectOfType<GameDifficulty>().mode = GameDifficulty.Mode.Normal;
        StartGame();
    }

    public void HardMode() {
        FindObjectOfType<GameDifficulty>().mode = GameDifficulty.Mode.Impossible;
        StartGame();
    }

    private void Start()
    {
        fadeCanvas.alpha = 1;
        fadeCanvas.gameObject.SetActive(true);
        fadeTimer = Time.time;
        menuStarting = true;
    }

    private void Update()
    {
        if (menuStarting) {
            float timeRatio = 1 - (Time.time - fadeTimer) / startingUpFadeDelay;
            fadeCanvas.alpha = timeRatio;

            if (timeRatio <= 0) {
                fadeCanvas.gameObject.SetActive(false);
                menuStarting = false;
            }
        }

        if (gameIsStarting) {
            float timeRatio = (Time.time - fadeTimer) / fadeDelay;
            fadeCanvas.alpha = timeRatio;

            if (timeRatio >= delayBeforeStarting) {
                SceneManager.LoadScene(1);
                gameIsStarting = false;
            }
        }
    }

    void StartGame() {
        gameIsStarting = true;
        fadeTimer = Time.time;
        fadeCanvas.gameObject.SetActive(true);
    }
}
