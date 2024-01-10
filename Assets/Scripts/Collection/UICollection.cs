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

    private float _resultAlpha = 0.5f; // 결과창 보일 때 블러의 알파 값

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
        // 느낌표 터치하면 채집 성공 실패 여부 알려줌
        // 느낌표 터치하면 대기 애니메이션 재생 후 종료되면 성공 실패 여부 알려주는 것으로 수정
        _collection.CollectionLatency();
        //_collection.IsSuccess();
        _checkResultButton.gameObject.SetActive(false);
    }

    private void SetSuccessFrame(string season)
    {
        if(season == "WAS" || season == "WSF" || season == "WWS") // 여름&겨울
        {
            _successImage.sprite = _successImageFrame[2];

        }
        else if (season == "WSP" || season == "WSU" || season == "WSS") // 여름
        {
            _successImage.sprite = _successImageFrame[0];
        }
        else
        {
            _successImage.sprite = _successImageFrame[1];
        }
    }

    /// <summary>
    /// 채집 성공 시 UI </summary>
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
            _resultText.text = "잡았다!\n" + item.Name + "!";
        }
        else
        {
            _resultText.text = "잡았다! " + item.Name + "!";
        }

        _successImage.gameObject.SetActive(true);

        // 아이템 이미지 받아와서 띄우기
        _collectionItemImage.sprite = item.Image;
        _collectionItemText.text = item.Name;
        Debug.Log("채집한 아이템 이미지 띄움"+id);

        _fadeInOut.gameObject.SetActive(true);
        Tween.IamgeAlpha(_fadeInOut.gameObject, _resultAlpha, 0.1f, TweenMode.Quadratic);
    }

    /// <summary>
    /// 채집 실패 시 UI </summary>
    private void SetFail()
    {
        _pungImage.SetActive(true);

        _resultTextImage.gameObject.SetActive(true);
        _resultText.text = ",,, 놓쳐 버렸다,,,";

        _fadeInOut.gameObject.SetActive(true);
        Tween.IamgeAlpha(_fadeInOut.gameObject, _resultAlpha, 0.1f, TweenMode.Quadratic);
    }

    /// <summary>
    /// 채집 UI 모두 종료 </summary>
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
