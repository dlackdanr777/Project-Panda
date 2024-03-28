using Muks.DataBind;
using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStoryCollection : MonoBehaviour
{
    #region UICollection
    [SerializeField] private Image _fadeInOut;

    [SerializeField] private Image _successImage;
    [SerializeField] private Sprite[] _successImageFrame;

    [SerializeField] private Image _collectionItemImage;
    [SerializeField] private TextMeshProUGUI _collectionItemText;

    [SerializeField] private Image _resultTextImage;
    [SerializeField] private TextMeshProUGUI _resultText;

    [SerializeField] private RectTransform[] _checkResultButtonPosition = new RectTransform[3];
    [SerializeField] private Button _checkResultButton;
    [SerializeField] private GameObject _pungImage;
    #endregion

    private float _resultAlpha = 0.5f; // 결과창 보일 때 블러의 알파 값

    #region Collection
    //[SerializeField] private CollectionButton _collectionButton;
    private Dictionary<string, MainStoryCollection> _collectionDic = new Dictionary<string, MainStoryCollection>();
    [SerializeField] private MainStoryCollection[] _collection;
    [SerializeField] private string[] _dictionaryId;
    #endregion



    private GatheringItemType _gathringItemType;
    private string _storyId;

    void Start()
    {
        //_collectionButton.OnCollectionButtonClicked += FadeInOut;

        for (int i = 0; i < _collection.Length; i++)
        {
            _collectionDic.Add(_dictionaryId[i], _collection[i]);
        }

        foreach (string key in _collectionDic.Keys)
        {
            _collectionDic[key].OnCollectionButtonClicked += FadeInOut;
            _collectionDic[key].OnCollectionSuccess += SetSuccess;
            _collectionDic[key].OnCollectionFail += SetFail;
            _collectionDic[key].OnExitCollection += ExitUICollection;
        }

        DataBind.SetButtonValue("CheckStoryResultButton", OnCheckResultButton);
    }

    private void OnDestroy()
    {
        //_collectionButton.OnCollectionButtonClicked -= FadeInOut;

        foreach (string key in _collectionDic.Keys)
        {
            _collectionDic[key].OnCollectionButtonClicked -= FadeInOut;
            _collectionDic[key].OnCollectionSuccess -= SetSuccess;
            _collectionDic[key].OnCollectionFail -= SetFail;
            _collectionDic[key].OnExitCollection -= ExitUICollection;
        }
    }

    private void FadeInOut(float fadeTime, GatheringItemType gatheringItemType, string storyId)
    {
        _gathringItemType = gatheringItemType;
        _storyId = storyId;

        if (gatheringItemType != GatheringItemType.None)
        {
            _checkResultButton.GetComponent<RectTransform>().anchoredPosition = _checkResultButtonPosition[(int)gatheringItemType].anchoredPosition;
        }

        _fadeInOut.gameObject.SetActive(true);
        Tween.IamgeAlpha(_fadeInOut.gameObject, 1, fadeTime, TweenMode.Constant, () =>
        {
            Tween.IamgeAlpha(_fadeInOut.gameObject, 0, fadeTime, TweenMode.Quadratic, () =>
            {
                _fadeInOut.gameObject.SetActive(false);
            });
        });
    }

    private void OnCheckResultButton()
    {
        // 느낌표 터치하면 채집 성공 실패 여부 알려줌
        // 느낌표 터치하면 대기 애니메이션 재생 후 종료되면 성공 실패 여부 알려주는 것으로 수정
        _collectionDic[_storyId].CollectionLatency();
        //_collection.IsSuccess();
        _checkResultButton.gameObject.SetActive(false);
    }

    private void SetSuccessFrame(string season)
    {
        if (season == "WAS" || season == "WSF" || season == "WWS") // 여름&겨울
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

        ToolItem toolItem = DatabaseManager.Instance.ItemDatabase.ItemToolList.Find(item => item.Id.Equals(id));

        SetSuccessFrame("WAS");

        _resultTextImage.gameObject.SetActive(true);

        if (toolItem.Name.Length > 7)
        {
            _resultText.text = "잡았다!\n" + toolItem.Name + "!";
        }
        else
        {
            _resultText.text = "잡았다! " + toolItem.Name + "!";
        }

        _successImage.gameObject.SetActive(true);

        // 아이템 이미지 받아와서 띄우기
        _collectionItemImage.sprite = toolItem.Image;
        _collectionItemText.text = toolItem.Name;

        _fadeInOut.gameObject.SetActive(true);
        Tween.IamgeAlpha(_fadeInOut.gameObject, _resultAlpha, 0.1f, TweenMode.Quadratic);
    }

    /// <summary>
    /// 채집 실패 시 UI </summary>
    private void SetFail(GatheringItemType gatheringItemType)
    {
        _pungImage.SetActive(true);

        _resultTextImage.gameObject.SetActive(true);

        if (gatheringItemType == GatheringItemType.Bug)
        {
            _resultText.text = ",,, 놓쳐 버렸다,,,";
        }
        else if (gatheringItemType == GatheringItemType.Fish)
        {
            _resultText.text = ",,, 물고기가 도망갔다,,,";
        }

        _fadeInOut.gameObject.SetActive(true);
        Tween.IamgeAlpha(_fadeInOut.gameObject, _resultAlpha, 0.1f, TweenMode.Quadratic);
    }

    /// <summary>
    /// 채집 UI 모두 종료 </summary>
    private void ExitUICollection()
    {
        _pungImage.SetActive(false);
        _resultTextImage.gameObject.SetActive(false);
        _successImage.gameObject.SetActive(false);
    }
}
