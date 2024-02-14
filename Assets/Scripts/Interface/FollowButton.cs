using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FollowButton : MonoBehaviour
{
    public Image _image;

    private Vector3 _correctionPos;

    private Button _button;

    private Transform _targetTransform;


    public void Init(GameObject target, Vector3 correctionPos, Vector2 size, Sprite sprite,  UnityAction onClicked)
    {
        _button = GetComponent<Button>();
        _button.GetComponent<RectTransform>().sizeDelta = size;

        _image.sprite = sprite;
        _button.onClick.AddListener(onClicked);
        _correctionPos = correctionPos;
        _targetTransform = target.transform;
    }

    private void Update()
    {
        if (!_targetTransform.gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

        if (_correctionPos != null || _correctionPos != Vector3.zero)
        {
            transform.position = Camera.main.WorldToScreenPoint(_correctionPos + _targetTransform.transform.position);
        }
    }

}
