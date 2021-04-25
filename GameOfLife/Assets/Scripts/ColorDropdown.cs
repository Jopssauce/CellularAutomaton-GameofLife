using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ColorDropdown : MonoBehaviour
{
    public int initialValue = 0;
    [SerializeField]
    public List<Color> colors = new List<Color>();
    public TMP_Dropdown dropdown;
    public UnityEvent<Color> onValueChanged;

    private void Start()
    {
        dropdown.value = initialValue;
        dropdown.onValueChanged.AddListener(ValueChangedCallback);
    }

    void ValueChangedCallback(int value)
    {
        onValueChanged?.Invoke(colors[value]);
    }

}
