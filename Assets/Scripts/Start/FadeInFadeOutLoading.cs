using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
            StartCoroutine(StartFadeIn());
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

        Debug.Log("��!");

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
