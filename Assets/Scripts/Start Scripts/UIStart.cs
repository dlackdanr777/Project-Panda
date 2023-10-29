using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStart : MonoBehaviour
{
    [SerializeField] private Button _startBackgroundButton;

    [SerializeField] private List<UIStartList> _uiLoadingList;

    [SerializeField] private List<UIStartList> _uiFirstLoadingList;

    private int _lastIndex;
    private int _currentIndex;

    private void Awake()
    {
        _startBackgroundButton.onClick.AddListener(OnBackgroundButtonClickd);

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

    }

    private void Update()
    {
        if (GameManager.Instance.IsFirstStart)
        {
            _uiFirstLoadingList[_currentIndex].UIUpdate();
        }
        else
        {
            _uiLoadingList[_currentIndex].UIUpdate();
        }
            
    }

    private void OnBackgroundButtonClickd()
    {
        if(GameManager.Instance.IsFirstStart)
        {
            _uiFirstLoadingList[_currentIndex].UIStart();
        }
        else
        {
            _uiLoadingList[_currentIndex].UIStart();
        }      
    }

    public void UIEnd()
    {
        if (_currentIndex < _lastIndex)
        {
            _currentIndex++;
            OnBackgroundButtonClickd();
        }
    }
}
