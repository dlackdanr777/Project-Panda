using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBack : MonoBehaviour
{
    [SerializeField] private GameObject _panel;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClickBack);
    }

    private void OnClickBack()
    {
        _panel.SetActive(false);
    }
}
