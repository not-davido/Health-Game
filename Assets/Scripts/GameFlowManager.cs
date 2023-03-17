using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    private bool gameIsEnding;

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
            SceneManager.LoadScene(0);
        }
    }

    void EndGame(bool win) {
        gameIsEnding = true;
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<PlayerFailedEvent>(PlayerFailed);
    }

    void PlayerFailed(PlayerFailedEvent evt) => EndGame(false);
}
