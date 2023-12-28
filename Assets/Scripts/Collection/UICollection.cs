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

        _collection.OnCollectionSuccess += SetSuccessText;
        _collection.OnCollectionSuccess += SetSuccessImage;

        _collection.OnCollectionFail += SetFailText;
        _collection.OnCollectionFail += SetFailImage;

        DataBind.SetButtonValue("CheckResultButton", OnCheckResultButton);
    }

    private void OnDestroy()
    {
        _collectionButton.OnCollectionButtonClicked -= FadeInOut;

        _collection.OnCollectionSuccess -= SetSuccessText;
        _collection.OnCollectionSuccess -= SetSuccessImage;

        _collection.OnCollectionFail -= SetFailText;
        _collection.OnCollectionFail -= SetFailImage;
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

    private void SetSuccessText()
    {
        _resultTextImage.gameObject.SetActive(true);
        _resultText.text = "잡았다!"; // 나중에 채집한 아이템 이름 뒤에 추가

    }

    private void SetFailText()
    {
        _resultTextImage.gameObject.SetActive(true);
        _resultText.text = ",,, 놓쳐 버렸다,,,";
    }

    private void SetSuccessImage()
    {
        _successImage.gameObject.SetActive(true);
        // 아이템 이미지 받아와서 띄우기
    }

    private void SetFailImage()
    {
        _successImage.gameObject.SetActive(true);

    }
}
