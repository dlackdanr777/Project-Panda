using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;

public class FadeInFadeOutLoading : StartList
{
    [Tooltip("ĵ������ �ִ� UI")]
    [SerializeField] private GameObject _uiFirstLoading3;

    [Tooltip("���� �̹���")]
    [SerializeField] private Image _seedImage;

    [Space(20)]

    [Tooltip("���̵� �� ���̵� �ƿ� UI")]
    [SerializeField] private GameObject _maskImage;

    [Space(5)]

    [Tooltip("���̵� �� ������ ������ ����")]
    [SerializeField] private Vector3 _fadeInSize;

    [Tooltip("���̵� �� �ӵ�")]
    [SerializeField] private float _fadeInTime;

    [Space(5)]

    [Tooltip("���̵� �ƿ� ������ ������ ����")]
    [SerializeField] private Vector3 _fadeOutSize;

    [Tooltip("���̵� �ƿ� �ӵ�")]
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
            Debug.Log("����");
        }
        else
        {
            Debug.Log("�̹� ������ �Դϴ�.");
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
        Debug.Log("����");
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
