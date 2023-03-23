using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float degreesPerSecond = 180;
    [SerializeField] private float scaleSpeedWhenReached = 15;

    private Camera cam;
    private bool reached;

    private void Awake()
    {
        cam = Camera.main;
        EventManager.AddListener<GameCompletedEvent>(GameEnded);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Time.deltaTime * degreesPerSecond * Vector3.forward);

        if (reached) {
            Vector3 cameraCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));
            cameraCenter.z = 0;
            transform.position = Vector3.Lerp(transform.position, cameraCenter, Time.deltaTime * 3);
            transform.localScale += new Vector3(scaleSpeedWhenReached, scaleSpeedWhenReached) * Time.deltaTime;
        }
    }

    void GameEnded(GameCompletedEvent evt) {
        GetComponent<SpriteRenderer>().sortingOrder = 2;
        degreesPerSecond *= 3;
        reached = true;
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<GameCompletedEvent>(GameEnded);

    }
}
