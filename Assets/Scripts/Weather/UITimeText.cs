using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UITimeText : MonoBehaviour
{
    private Text _timeText;
    void Start()
    {
        _timeText = GetComponent<Text>();
        InvokeRepeating("SetTimeText", 0, 1);
    }

    void SetTimeText()
    {
        string timeText = UserInfo.TODAY.ToString("hh:mm tt", CultureInfo.InvariantCulture);
        _timeText.text = timeText;
    }
}
