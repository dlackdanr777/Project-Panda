using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICookingEnd : MonoBehaviour
{


    [SerializeField] private Image _layoutImage;
    [SerializeField] private Button _okButton;


    public void Init(UnityAction onButtonClicked)
    {
        _okButton.onClick.AddListener(onButtonClicked);
        _okButton.onClick.AddListener(() => gameObject.SetActive(false));
        gameObject.SetActive(false);
    }

    public void Show(Item item)
    {
        gameObject.SetActive(true);
    }


}
