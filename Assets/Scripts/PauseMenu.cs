using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuRoot;

    private bool canPause;

    public static bool GameIsPaused { get; private set; }

    // Start is called before the first frame update
    void Start() {
        SetPauseMenuActivation(false);
        canPause = true;
    }

    // Update is called once per frame
    void Update() {
        if (!canPause) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame ||
            Keyboard.current.pKey.wasPressedThisFrame) {
            SetPauseMenuActivation(!menuRoot.activeSelf);
        }
    }

    void SetPauseMenuActivation(bool active) {
        menuRoot.SetActive(active);

        Time.timeScale = menuRoot.activeSelf ? 0 : 1;
        GameIsPaused = active;
    }

    public void ClosePauseMenu() {
        SetPauseMenuActivation(false);
    }

    public void Quit() {
        canPause = false;

        GameDifficulty gameDifficulty = FindObjectOfType<GameDifficulty>();
        if (gameDifficulty != null) 
            Destroy(gameDifficulty.gameObject);

        menuRoot.SetActive(false);

        EventManager.Broadcast(Events.GameQuitEvent);
    }
}
