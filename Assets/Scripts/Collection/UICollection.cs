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
    [SerializeField] private Sprite[] _successImageFrame;

    [SerializeField] private Image _collectionItemImage;
    [SerializeField] private TextMeshProUGUI _collectionItemText;

    [SerializeField] private Image _resultTextImage;
    [SerializeField] private TextMeshProUGUI _resultText;

    [SerializeField] private Button _checkResultButton;
    [SerializeField] private GameObject _pungImage;
    #endregion

    #region Collection
    //[SerializeField] private CollectionButton _collectionButton;
    [SerializeField] private Collection _collection;
    #endregion

    private float _resultAlpha = 0.5f; // ���â ���� �� ���� ���� ��

    void Start()
    {
        //_collectionButton.OnCollectionButtonClicked += FadeInOut;
        _collection.OnCollectionButtonClicked += FadeInOut;

        _collection.OnCollectionSuccess += SetSuccess;
        _collection.OnCollectionFail += SetFail;

        _collection.OnExitCollection += ExitUICollection;

        DataBind.SetButtonValue("CheckResultButton", OnCheckResultButton);
    }

    private void OnDestroy()
    {
        //_collectionButton.OnCollectionButtonClicked -= FadeInOut;
        _collection.OnCollectionButtonClicked -= FadeInOut;

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

    private void DesplayResultText()
    {

    }

    private void OnCheckResultButton()
    {
        // ����ǥ ��ġ�ϸ� ä�� ���� ���� ���� �˷���
        // ����ǥ ��ġ�ϸ� ��� �ִϸ��̼� ��� �� ����Ǹ� ���� ���� ���� �˷��ִ� ������ ����
        _collection.CollectionLatency();
        //_collection.IsSuccess();
        _checkResultButton.gameObject.SetActive(false);
    }

    private void SetSuccessFrame(string season)
    {
        if(season == "WAS" || season == "WSF" || season == "WWS") // ����&�ܿ�
        {
            _successImage.sprite = _successImageFrame[2];

        }
        else if (season == "WSP" || season == "WSU" || season == "WSS") // ����
        {
            _successImage.sprite = _successImageFrame[0];
        }
        else
        {
            _successImage.sprite = _successImageFrame[1];
        }
    }

    /// <summary>
    /// ä�� ���� �� UI </summary>
    private void SetSuccess(string id)
    {
        _pungImage.SetActive(true);

        Item item = DatabaseManager.Instance.GetBugItemList().Find(item => item.Id.Equals(id));

        if(item is GatheringItem gatheringItem)
        {
            SetSuccessFrame(gatheringItem.Season);
        }

        _resultTextImage.gameObject.SetActive(true);
        if(item.Name.Length > 7)
        {
            _resultText.text = "��Ҵ�!\n" + item.Name + "!";
        }
        else
        {
            _resultText.text = "��Ҵ�! " + item.Name + "!";
        }

        _successImage.gameObject.SetActive(true);

        // ������ �̹��� �޾ƿͼ� ����
        _collectionItemImage.sprite = item.Image;
        _collectionItemText.text = item.Name;
        Debug.Log("ä���� ������ �̹��� ���"+id);

        _fadeInOut.gameObject.SetActive(true);
        Tween.IamgeAlpha(_fadeInOut.gameObject, _resultAlpha, 0.1f, TweenMode.Quadratic);
    }

    /// <summary>
    /// ä�� ���� �� UI </summary>
    private void SetFail()
    {
        _pungImage.SetActive(true);

        _resultTextImage.gameObject.SetActive(true);
        _resultText.text = ",,, ���� ���ȴ�,,,";

        _fadeInOut.gameObject.SetActive(true);
        Tween.IamgeAlpha(_fadeInOut.gameObject, _resultAlpha, 0.1f, TweenMode.Quadratic);
    }

    /// <summary>
    /// ä�� UI ��� ���� </summary>
    private void ExitUICollection()
    {
        _pungImage.SetActive(false);

        Tween.IamgeAlpha(_fadeInOut.gameObject, 0, 0.1f, TweenMode.Quadratic, () =>
        {
            _fadeInOut.gameObject.SetActive(false);

        });
        _resultTextImage.gameObject.SetActive(false);
        _successImage.gameObject.SetActive(false);
    }
}
