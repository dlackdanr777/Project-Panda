using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
            StartCoroutine(StartFadeIn());
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

    private IEnumerator StartFadeIn()
    {
        _seedImage.gameObject.SetActive(false);
        float timer = 0;
        Vector3 tempScale = _maskImage.transform.localScale;

        while(timer <  _fadeInTime)
        {
            timer += Time.deltaTime;
            float t = timer / _fadeInTime;
            t = t * t * (3f - 2f * t);

            _maskImage.transform.localScale = 
                Vector3.Lerp(tempScale, _fadeInSize, t);

            yield return null;
        }

        _seedImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(5);

        yield return StartCoroutine(StartFadeOut());

        Debug.Log("끝!");

    }

    private IEnumerator StartFadeOut()
    {
        float timer = 0;
        Vector3 tempScale = _maskImage.transform.localScale;

        while (timer < _fadeOutTime)
        {
            timer += Time.deltaTime;
            float t = timer / _fadeOutTime;
            t = t * t * (3f - 2f * t);

            _maskImage.transform.localScale =
                Vector3.Lerp(tempScale, _fadeOutSize, t);

            Color color = _seedImage.color;
            color.a = Mathf.Abs(1 - (timer / _fadeOutTime));
            _seedImage.color = color;

            yield return null;
        }
    }


}
