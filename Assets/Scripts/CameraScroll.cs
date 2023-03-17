using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [SerializeField] private float speed = 3;

    private bool scrolling;

    private void Awake()
    {
        StartGame.OnGameStarted += StartScroll;
    }

    // Update is called once per frame
    void Update()
    {
        if (scrolling) {
            transform.Translate(speed * Time.deltaTime * Vector3.up);
        }
    }

    void StartScroll() {
        scrolling = true;
    }

    private void OnDisable()
    {
        StartGame.OnGameStarted -= StartScroll;
    }
}
