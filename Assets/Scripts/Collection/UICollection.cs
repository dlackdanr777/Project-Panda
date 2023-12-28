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
    /// 채집 성공 시 UI </summary>
    private void SetSuccess()
    {
        _resultTextImage.gameObject.SetActive(true);
        _resultText.text = "잡았다!"; // 나중에 채집한 아이템 이름 뒤에 추가

        _successImage.gameObject.SetActive(true);
        // 아이템 이미지 받아와서 띄우기

    }

    /// <summary>
    /// 채집 실패 시 UI </summary>
    private void SetFail()
    {
        _resultTextImage.gameObject.SetActive(true);
        _resultText.text = ",,, 놓쳐 버렸다,,,";

        _successImage.gameObject.SetActive(true);
    }

    /// <summary>
    /// 채집 UI 모두 종료 </summary>
    private void ExitUICollection()
    {
        _resultTextImage.gameObject.SetActive(false);
        _successImage.gameObject.SetActive(false);
    }
}
