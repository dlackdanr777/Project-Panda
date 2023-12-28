using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using Muks.DataBind;

public class UICollection : MonoBehaviour
{
    #region UICollection
    [SerializeField] private Image _fadeInOut;
    [SerializeField] private Image _successImage;
    [SerializeField] private Image _resultTextImage;
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private Button _checkResultButton;
    #endregion

    #region Collection
    [SerializeField] private CollectionButton _collectionButton;
    [SerializeField] private Collection _collection;
    #endregion

    void Start()
    {
        _collectionButton.OnCollectionButtonClicked += FadeInOut;

        _collection.OnCollectionSuccess += SetSuccess;
        _collection.OnCollectionFail += SetFail;

        _collection.OnExitCollection += ExitUICollection;

        DataBind.SetButtonValue("CheckResultButton", OnCheckResultButton);
    }

    private void OnDestroy()
    {
        _collectionButton.OnCollectionButtonClicked -= FadeInOut;

        _collection.OnCollectionSuccess -= SetSuccess;
        _collection.OnCollectionFail -= SetFail;

        _collection.OnExitCollection -= ExitUICollection;

    }

    private void FadeInOut(float fadeTime)
    {
        _fadeInOut.gameObject.SetActive(true);
        Tween.IamgeAlpha(_fadeInOut.gameObject, 1, fadeTime, TweenMode.Quadratic, () =>
        {
            Tween.IamgeAlpha(_fadeInOut.gameObject, 0, fadeTime, TweenMode.Quadratic, () =>
            {
                _fadeInOut.gameObject.SetActive(false);
            });
        });
    }

    private void DesplaySesultText()
    {

    }

    private void OnCheckResultButton()
    {
        _collection.IsSuccess();
        _checkResultButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// ä�� ���� �� UI </summary>
    private void SetSuccess()
    {
        _resultTextImage.gameObject.SetActive(true);
        _resultText.text = "��Ҵ�!"; // ���߿� ä���� ������ �̸� �ڿ� �߰�

        _successImage.gameObject.SetActive(true);
        // ������ �̹��� �޾ƿͼ� ����

    }

    /// <summary>
    /// ä�� ���� �� UI </summary>
    private void SetFail()
    {
        _resultTextImage.gameObject.SetActive(true);
        _resultText.text = ",,, ���� ���ȴ�,,,";

        _successImage.gameObject.SetActive(true);
    }

    /// <summary>
    /// ä�� UI ��� ���� </summary>
    private void ExitUICollection()
    {
        _resultTextImage.gameObject.SetActive(false);
        _successImage.gameObject.SetActive(false);
    }
}
