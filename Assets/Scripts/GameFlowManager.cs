using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private TMPro.TextMeshProUGUI gameResultText;
    [SerializeField] private float fadeDelay = 1.5f;
    [SerializeField] private float delayBeforeFading = 3;
    [SerializeField] private float extraDelayForWin = 2f;
    [SerializeField] private float quitFadeDelay = 1f;
    [SerializeField] private float delayBeforeQuiting = 0.5f;

    private GameDifficulty GameDifficulty;
    private Attempts Attempts;
    private bool gameIsStarting;
    private bool gameIsEnding;
    private bool gameIsQuiting;
    private bool won;
    private float fadeTimer;

    public bool GameIsEnding => gameIsEnding;

    private void Awake()
    {
        EventManager.AddListener<PlayerFailedEvent>(PlayerFailed);
        EventManager.AddListener<GameQuitEvent>(OnQuit);
        EventManager.AddListener<GameCompletedEvent>(GameCompleted);
    }

    private void Start()
    {
        GameDifficulty = FindObjectOfType<GameDifficulty>();
        Attempts = FindObjectOfType<Attempts>();

        gameIsStarting = true;
        fadeTimer = Time.time;
        fadeCanvas.gameObject.SetActive(true);
        gameResultText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsStarting) {
            float timeRatio = 1 - (Time.time - fadeTimer) / fadeDelay;
            fadeCanvas.alpha = timeRatio;

            if (timeRatio <= 0) {
                gameIsStarting = false;
            }
        }

        if (gameIsEnding) {
            float timeRatio = (Time.time - fadeTimer) / fadeDelay;
            fadeCanvas.alpha = timeRatio;

            if (won) {
                if (timeRatio >= 1 + 0.5f) {
                    gameResultText.gameObject.SetActive(true);
                    gameResultText.text = $"Difficulty : <color={GameDifficulty.textColor}>{GameDifficulty.mode}</color>\n Attempts : {Attempts.attempt}";

                    if (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame) {
                        SceneManager.LoadScene(0);
                    }
                }
            } else {
                if (!PauseMenu.GameIsPaused && (Keyboard.current.spaceKey.wasPressedThisFrame ||
                        Mouse.current.leftButton.wasPressedThisFrame)) {
                    SceneManager.LoadScene(1);
                }

                if (timeRatio >= 1)
                    SceneManager.LoadScene(1);
            }
        }

        if (gameIsQuiting) {
            float timeRatio = (Time.unscaledTime - fadeTimer) / quitFadeDelay;
            fadeCanvas.alpha = timeRatio;

            if (timeRatio >= 1) {
                SceneManager.LoadScene(0);
                Time.timeScale = 1;
            }  
        }
    }

    void EndGame(bool win) {
        gameIsEnding = true;
        fadeCanvas.gameObject.SetActive(true);
        won = win;

        if (win) {
            fadeTimer = Time.time + delayBeforeFading + extraDelayForWin;
        } else {
            fadeTimer = Time.time + delayBeforeFading;
        }
    }

    void QuitGame() {
        gameIsQuiting = true;
        fadeCanvas.gameObject.SetActive(true);
        fadeTimer = Time.unscaledTime + delayBeforeQuiting;
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<PlayerFailedEvent>(PlayerFailed);
        EventManager.RemoveListener<GameQuitEvent>(OnQuit);
        EventManager.RemoveListener<GameCompletedEvent>(GameCompleted);

    }

    void PlayerFailed(PlayerFailedEvent evt) => EndGame(false);
    void OnQuit(GameQuitEvent evt) => QuitGame();
    void GameCompleted(GameCompletedEvent evt) => EndGame(true);
}
