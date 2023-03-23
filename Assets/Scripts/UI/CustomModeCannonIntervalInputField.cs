using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomModeCannonIntervalInputField : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField fromInputField;
    [SerializeField] private TMPro.TMP_InputField toInputField;

    public float FROM_VALUE { get; private set; }
    public float TO_VALUE { get; private set; }

    private void OnEnable() {
        fromInputField.characterValidation = TMPro.TMP_InputField.CharacterValidation.Decimal;
        toInputField.characterValidation = TMPro.TMP_InputField.CharacterValidation.Decimal;

        fromInputField.onEndEdit.AddListener(UpdateFromInput);
        toInputField.onValueChanged.AddListener(ToValueChanged);
        toInputField.onEndEdit.AddListener(UpdateToInput);

        UpdateFromInput(fromInputField.text);
        UpdateToInput(toInputField.text);
    }

    void UpdateFromInput(string value) {
        if (value == string.Empty || value == null) {
            fromInputField.text = "1";
            FROM_VALUE = 1;
        } else {
            float from = float.Parse(value);

            if (from == 0) {
                from = 0.1f;
                fromInputField.text = from.ToString();
            } else if (from < 0) {
                from = Mathf.Abs(from);
                fromInputField.text = from.ToString();
            }

            float to = float.Parse(toInputField.text);

            if (from > to) {
                from = to;
                fromInputField.text = from.ToString();
            }

            FROM_VALUE = from;
        }
    }

    void UpdateToInput(string value) {
        if (value == string.Empty || value == null) {
            toInputField.text = "1";
            TO_VALUE = 1;
        } else {
            float to = float.Parse(value);

            if (to == 0) {
                to = 0.1f;
                toInputField.text = to.ToString();
            } else if (to < 0) {
                to = Mathf.Abs(to);
                toInputField.text = to.ToString();
            }

            float from = float.Parse(fromInputField.text);

            if (to < from) {
                to = from;
                toInputField.text = to.ToString();
            }

            TO_VALUE = to;
        }
    }

    void ToValueChanged(string value) {
        if (float.TryParse(value, out float n)) {
            if (n > 5) {
                n = 5;
                toInputField.text = n.ToString();
            }
        }
    }

    private void OnDisable() {
        fromInputField.onEndEdit.RemoveListener(UpdateFromInput);
        toInputField.onValueChanged.RemoveListener(ToValueChanged);
        toInputField.onEndEdit.RemoveListener(UpdateToInput);
    }
}