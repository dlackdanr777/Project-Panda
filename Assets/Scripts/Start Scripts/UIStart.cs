using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStart : MonoBehaviour
{
    [Tooltip("배경 버튼")]
    [SerializeField] private Button _startBackgroundButton;

    [Tooltip("기존 유저 로그인시 나타나게될 UI 순서")]
    [SerializeField] private List<UIStartList> _uiLoadingList;

    [Tooltip("신규 유저 로그인시 나타나게될 UI 순서")]
    [SerializeField] private List<UIStartList> _uiFirstLoadingList;

    private UIStartList _currentUI;

    private int _lastIndex;
    private int _nextIndex;

    private void Awake()
    {
        Init();
        _startBackgroundButton.onClick.AddListener(OnBackgroundButtonClickd);
    }

    private void Update()
    {
            _currentUI?.UIUpdate();
    }


    private void Init()
    {
        if (GameManager.Instance.IsFirstStart)
        {
            _lastIndex = _uiFirstLoadingList.Count;
            for (int i = 0; i < _lastIndex; i++)
            {
                _uiFirstLoadingList[i].Init(this);
            }
        }
        else
        {
            _lastIndex = _uiLoadingList.Count;
            for (int i = 0; i < _lastIndex; i++)
            {
                _uiLoadingList[i].Init(this);
            }
        }

        ChangeCurrentUI();
    }


    private void OnBackgroundButtonClickd()
    {
        _currentUI?.UIStart();
    }


    public void ChangeCurrentUI()
    {
        if (_nextIndex < _lastIndex)
        {
            if (GameManager.Instance.IsFirstStart)
            {
                _currentUI = _uiFirstLoadingList[_nextIndex];
            }
            else
            {
                _currentUI = _uiLoadingList[_nextIndex];
            }
            _nextIndex++;
            OnBackgroundButtonClickd();
        }
    }
}
