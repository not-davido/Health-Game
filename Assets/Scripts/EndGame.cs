using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private bool gameCompleted;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameCompleted) return;

        if (collision.TryGetComponent(out PlayerController2D _)) {
            EventManager.Broadcast(Events.GameCompletedEvent);
            gameCompleted = true;
        }
    }
}
