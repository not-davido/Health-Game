using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    private bool gameStarted;

    public static event Action OnGameStarted;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameStarted) return;

        if (collision.TryGetComponent(out PlayerController2D _)) {
            OnGameStarted?.Invoke();
            gameStarted = true;
        }
    }
}
