using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private float delay = 5;

    private bool gameIsEnding;
    private float delayTimer;

    public bool GameIsEnding => gameIsEnding;

    private void Awake()
    {
        EventManager.AddListener<PlayerFailedEvent>(PlayerFailed);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsEnding) {
            if (Time.time > delayTimer)
                SceneManager.LoadScene(0);
        }
    }

    void EndGame(bool win) {
        gameIsEnding = true;

        if (!win) {
            delayTimer = Time.time + delay;
        }
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<PlayerFailedEvent>(PlayerFailed);
    }

    void PlayerFailed(PlayerFailedEvent evt) => EndGame(false);
}
