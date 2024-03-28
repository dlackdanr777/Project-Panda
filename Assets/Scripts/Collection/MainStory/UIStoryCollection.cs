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

    private float _resultAlpha = 0.5f; // ���â ���� �� ���� ���� ��

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
        // ����ǥ ��ġ�ϸ� ä�� ���� ���� ���� �˷���
        // ����ǥ ��ġ�ϸ� ��� �ִϸ��̼� ��� �� ����Ǹ� ���� ���� ���� �˷��ִ� ������ ����
        _collectionDic[_storyId].CollectionLatency();
        //_collection.IsSuccess();
        _checkResultButton.gameObject.SetActive(false);
    }

    private void SetSuccessFrame(string season)
    {
        if (season == "WAS" || season == "WSF" || season == "WWS") // ����&�ܿ�
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

        ToolItem toolItem = DatabaseManager.Instance.ItemDatabase.ItemToolList.Find(item => item.Id.Equals(id));

        SetSuccessFrame("WAS");

        _resultTextImage.gameObject.SetActive(true);

        if (toolItem.Name.Length > 7)
        {
            _resultText.text = "��Ҵ�!\n" + toolItem.Name + "!";
        }
        else
        {
            _resultText.text = "��Ҵ�! " + toolItem.Name + "!";
        }

        _successImage.gameObject.SetActive(true);

        // ������ �̹��� �޾ƿͼ� ����
        _collectionItemImage.sprite = toolItem.Image;
        _collectionItemText.text = toolItem.Name;

        _fadeInOut.gameObject.SetActive(true);
        Tween.IamgeAlpha(_fadeInOut.gameObject, _resultAlpha, 0.1f, TweenMode.Quadratic);
    }

    /// <summary>
    /// ä�� ���� �� UI </summary>
    private void SetFail(GatheringItemType gatheringItemType)
    {
        _pungImage.SetActive(true);

        _resultTextImage.gameObject.SetActive(true);

        if (gatheringItemType == GatheringItemType.Bug)
        {
            _resultText.text = ",,, ���� ���ȴ�,,,";
        }
        else if (gatheringItemType == GatheringItemType.Fish)
        {
            _resultText.text = ",,, ����Ⱑ ��������,,,";
        }

        _fadeInOut.gameObject.SetActive(true);
        Tween.IamgeAlpha(_fadeInOut.gameObject, _resultAlpha, 0.1f, TweenMode.Quadratic);
    }

    /// <summary>
    /// ä�� UI ��� ���� </summary>
    private void ExitUICollection()
    {
        _pungImage.SetActive(false);
        _resultTextImage.gameObject.SetActive(false);
        _successImage.gameObject.SetActive(false);
    }
}
