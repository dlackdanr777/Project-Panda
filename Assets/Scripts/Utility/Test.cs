using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private string _value;

    // Update is called once per frame
    void Update()
    {
        if (!string.IsNullOrEmpty(_value))
        {
            UIView.SetValue("Test ID", _value);
        }
    }
}
