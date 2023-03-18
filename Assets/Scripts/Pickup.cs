using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Tooltip("Frequency at which the item will move up and down")]
    public float VerticalBobFrequency = 1f;

    [Tooltip("Distance the item will move up and down")]
    public float BobbingAmount = 1f;

    [Tooltip("Rotation angle per second")] public float RotatingSpeed = 360f;

    [Tooltip("Sound played on pickup")] public AudioClip PickupSfx;
    [Tooltip("VFX spawned on pickup")] public GameObject PickupVfxPrefab;

    Collider2D m_Collider;
    Vector3 m_StartPosition;
    bool m_HasPlayedFeedback;

    protected virtual void Start() {
        m_Collider = GetComponent<Collider2D>();
        m_Collider.isTrigger = true;

        // Remember start position for animation
        m_StartPosition = transform.position;
    }

    void Update() {
        // Handle bobbing
        float bobbingAnimationPhase = ((Mathf.Sin(Time.time * VerticalBobFrequency) * 0.5f) + 0.5f) * BobbingAmount;
        transform.position = m_StartPosition + Vector3.up * bobbingAnimationPhase;

        // Handle rotating
        transform.Rotate(Vector3.forward, RotatingSpeed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController2D>(out var pickingPlayer)) {
            OnPicked(pickingPlayer);

            PickupEvent evt = Events.PickupEvent;
            evt.Pickup = gameObject;
            EventManager.Broadcast(evt);
        }
    }

    protected virtual void OnPicked(PlayerController2D playerController) {
        PlayPickupFeedback();
    }

    public void PlayPickupFeedback() {
        if (m_HasPlayedFeedback)
            return;

        //if (PickupSfx)
        //{
        //    AudioUtility.CreateSFX(PickupSfx, transform.position, AudioUtility.AudioGroups.Pickup, 0f);
        //}

        if (PickupVfxPrefab) {
            var pickupVfxInstance = Instantiate(PickupVfxPrefab, transform.position, Quaternion.identity);
        }

        m_HasPlayedFeedback = true;
    }
}
