using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeRuntime : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI sample;

    // Start is called before the first frame update
    void Start()
    {
        sample.text = "The color is <color=red>red</color>.\nThe color is <color=green>green</color>.\nThe color is <color=blue>blue</color>.";
    }
}
