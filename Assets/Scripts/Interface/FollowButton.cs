using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FollowButton : MonoBehaviour
{
    [SerializeField] private Image _image;

    private Vector3 _targetPos;

    private Button _button;

    private GameObject _targetGameObject;


    public void Init(GameObject target, Vector3 targetPos, Vector2 size, Sprite sprite,  Action onClicked)
    {
        _button = GetComponent<Button>();

        _button.GetComponent<RectTransform>().sizeDelta = size;
        _image.sprite = sprite;
        _button.onClick.AddListener(() => onClicked?.Invoke());
        _targetPos = targetPos;

        _targetGameObject = target;
    }

    private void Update()
    {
        if (!_targetGameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

        if (_targetPos != null || _targetPos != Vector3.zero)
        {
            transform.position = Camera.main.WorldToScreenPoint(_targetPos);
        }
    }

}
