using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMode : MonoBehaviour
{
    public int HealthPoints { get; set; }
    public float LightRadius { get; set; }
    public float ScrollSpeed { get; set; }
    public Vector2 interval { get; set; }

    private void Awake()
    {
        EventManager.AddListener<GameQuitEvent>(OnQuit);
        //EventManager.AddListener<GameCompletedEvent>(GameEnded);
    }

    private void Start()
    {
        Debug.Log(interval);
    }

    void OnQuit(GameQuitEvent evt) {
        Destroy(gameObject);
    }

    //void GameEnded(GameCompletedEvent evt) {
    //    Destroy(gameObject);
    //}

    private void OnDisable()
    {
        EventManager.RemoveListener<GameQuitEvent>(OnQuit);
        //EventManager.RemoveListener<GameCompletedEvent>(GameEnded);
    }
}
