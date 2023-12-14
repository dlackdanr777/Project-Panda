using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectToCanvas : MonoBehaviour
{
    [SerializeField] GameObject _wood;

    private bool isOn;
    private Vector3 _screenPosition;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickArrange);       
        _screenPosition = _wood.transform.position;
    }

    private void OnClickArrange()
    {
        if (!isOn)
        {
            Vector3 canvasPosition = Camera.main.WorldToScreenPoint(_wood.transform.position);
            canvasPosition.z = 10;
            _wood.transform.position = canvasPosition;
            isOn = true;
        }
        else
        {
            _wood.transform .position = _screenPosition;
            isOn = false;
        }
    }
}
