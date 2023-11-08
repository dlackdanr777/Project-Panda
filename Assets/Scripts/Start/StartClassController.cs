using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartClassController : MonoBehaviour
{
    [Tooltip("��� ��ư")]
    [SerializeField] private Button _startBackgroundButton;

    [Tooltip("���� ���� �α��ν� ��Ÿ���Ե� UI ����")]
    [SerializeField] private List<StartList> _startLoadingList;

    [Tooltip("�ű� ���� �α��ν� ��Ÿ���Ե� UI ����")]
    [SerializeField] private List<StartList> _firstStartLoadingList;

    private StartList _currentClass;

    private int _lastIndex;
    private int _nextIndex;

    private void Awake()
    {
        Init();
        _startBackgroundButton.onClick.AddListener(OnBackgroundButtonClickd);
    }

    private void Update()
    {
            _currentClass?.UIUpdate();
    }


    private void Init()
    {
        if (GameManager.Instance.IsFirstStart)
        {
            _lastIndex = _firstStartLoadingList.Count;
            for (int i = 0; i < _lastIndex; i++)
            {
                _firstStartLoadingList[i].Init(this);
            }
        }
        else
        {
            _lastIndex = _startLoadingList.Count;
            for (int i = 0; i < _lastIndex; i++)
            {
                _startLoadingList[i].Init(this);
            }
        }

        ChangeCurrentClass();
    }


    private void OnBackgroundButtonClickd()
    {
        _currentClass?.UIStart();
    }


    public void ChangeCurrentClass()
    {
        if (_nextIndex < _lastIndex)
        {
            if (GameManager.Instance.IsFirstStart)
            {
                _currentClass = _firstStartLoadingList[_nextIndex];
            }
            else
            {
                _currentClass = _startLoadingList[_nextIndex];
            }
            _nextIndex++;
            OnBackgroundButtonClickd();
        }
    }
}