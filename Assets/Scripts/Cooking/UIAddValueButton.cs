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


    /// <summary>클릭 시 사용 가능, 불가 여부를 받아 버튼 이벤트를 변경하는 함수</summary>
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


    /// <summary>버튼을 사용할 수 없을 때 사용불가 애니메이션을 실행하는 함수</summary>
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
