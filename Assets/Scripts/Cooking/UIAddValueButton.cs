using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Muks.Tween;

public class UIAddValueButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    public Button Button => _button;


    public void Start()
    {
        _button.onClick.AddListener(NotUsabledAnimation); 
    }


    /// <summary>Ŭ�� �� ��� ����, �Ұ� ���θ� �޾� ��ư �̺�Ʈ�� �����ϴ� �Լ�</summary>
    public void CheckUsabled(bool isUsabled, UnityAction onClick)
    {
        _button.onClick.RemoveAllListeners();

        if (isUsabled)
        {
            _button.onClick.AddListener(onClick);
        }
        else
        {
            _button.onClick.AddListener(NotUsabledAnimation);
        }
    }


    /// <summary>��ư�� ����� �� ���� �� ���Ұ� �ִϸ��̼��� �����ϴ� �Լ�</summary>
    private void NotUsabledAnimation()
    {
        Tween.Stop(_button.gameObject);

        Vector3 targetPos1 = _button.gameObject.transform.position + new Vector3(3f, 3, 0);
        Vector3 targetPos2 = _button.gameObject.transform.position + new Vector3(3f, 3, 0);

        Tween.TransformMove(_button.gameObject, targetPos1, 0.1f, TweenMode.Spike);
        Tween.TransformMove(_button.gameObject, targetPos2, 0.1f, TweenMode.Spike);
        Tween.TransformMove(_button.gameObject, targetPos1, 0.1f, TweenMode.Spike);
        Tween.TransformMove(_button.gameObject, targetPos2, 0.1f, TweenMode.Spike);
        Tween.TransformMove(_button.gameObject, targetPos1, 0.1f, TweenMode.Spike);
    }
}
