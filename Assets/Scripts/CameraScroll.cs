using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [SerializeField] private float speed = 3;

    private bool scrolling;

    private void Awake()
    {
        StartGame.OnGameStarted += Scroll;

        EventManager.AddListener<PlayerFailedEvent>(OnPlayerFailed);
    }

    // Update is called once per frame
    void Update()
    {
        if (scrolling) {
            transform.Translate(speed * Time.deltaTime * Vector3.up);
        }
    }

    void Scroll(bool scroll) {
        scrolling = scroll;
    }

    void OnPlayerFailed(PlayerFailedEvent evt) => Scroll(false);

    private void OnDisable()
    {
        StartGame.OnGameStarted -= Scroll;

        EventManager.RemoveListener<PlayerFailedEvent>(OnPlayerFailed);
    }
}
