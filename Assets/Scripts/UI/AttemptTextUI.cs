using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttemptTextUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        Attempts a = FindObjectOfType<Attempts>();

        if (a != null) {
            text.text = $"Attempt {a.attempt}";
        }
    }
}
