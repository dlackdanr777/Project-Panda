using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;

public class FadeInFadeOutLoading : StartList
{
    [Tooltip("캔버스에 있는 UI")]
    [SerializeField] private GameObject _uiFirstLoading3;

    [Tooltip("씨앗 이미지")]
    [SerializeField] private Image _seedImage;

    [Space(20)]

    [Tooltip("페이드 인 페이드 아웃 UI")]
    [SerializeField] private GameObject _maskImage;

    [Space(5)]

    [Tooltip("페이드 인 스케일 사이즈 설정")]
    [SerializeField] private Vector3 _fadeInSize;

    [Tooltip("페이드 인 속도")]
    [SerializeField] private float _fadeInTime;

    [Space(5)]

    [Tooltip("페이드 아웃 스케일 사이즈 설정")]
    [SerializeField] private Vector3 _fadeOutSize;

    [Tooltip("페이드 아웃 속도")]
    [SerializeField] private float _fadeOutTime;


    private StartClassController _uiStart;

    private bool _isStart;

    public override void Init(StartClassController uiStart)
    {
        _uiStart = uiStart;
        _uiFirstLoading3.SetActive(false);
    }

    public override void UIStart()
    {
        if (!_isStart)
        {
            _isStart = true;
            _uiFirstLoading3.SetActive(true);
            StartFadeIn();
            Debug.Log("시작");
        }
        else
        {
            Debug.Log("이미 실행중 입니다.");
        }
        
    }
    public override void UIUpdate()
    {
    }

    public override void UIEnd()
    {
        _uiStart?.ChangeCurrentClass();
    }

    private void StartFadeIn()
    {
        _seedImage.gameObject.SetActive(false);
        Debug.Log("실행");
        Tween.TransformScale(_maskImage, _fadeInSize, _fadeInTime, TweenMode.Smootherstep, SeedEnable);
    }

    private void SeedEnable()
    {
        _seedImage.gameObject.SetActive(true);
        Tween.TransformScale(_seedImage.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 1, TweenMode.Spike);
        Tween.TransformScale(_seedImage.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 1, TweenMode.Spike);
        Tween.TransformScale(_seedImage.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 1, TweenMode.Spike);
        Tween.TransformScale(_seedImage.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 1, TweenMode.Spike);
        Tween.TransformScale(_seedImage.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 1, TweenMode.Spike, StartFadeOut);
    }

    private void StartFadeOut()
    {
        Color targetColor = _seedImage.color;
        targetColor.a = 0;
        
        Tween.TransformScale(_maskImage, _fadeOutSize, _fadeOutTime, TweenMode.Constant);
        Tween.IamgeColor(_seedImage.gameObject, targetColor, _fadeOutTime * 0.5f, TweenMode.Constant);
    }


}
