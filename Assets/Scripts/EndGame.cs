using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private AudioClip endSFX;

    private bool gameCompleted;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameCompleted) return;

        if (collision.TryGetComponent(out PlayerController2D player)) {
            GetComponent<AudioSource>().PlayOneShot(endSFX);
            player.GetComponent<PlayerInputHandler>().StopMove();
            EventManager.Broadcast(Events.GameCompletedEvent);
            gameCompleted = true;
        }
    }
}
