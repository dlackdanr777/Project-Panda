using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartClassController : MonoBehaviour
{
    [Tooltip("배경 버튼")]
    [SerializeField] private Button _backgroundButton;

    [Tooltip("기존 유저 로그인시 나타나게될 UI 순서")]
    [SerializeField] private List<StartClass> _existingUserStartList;

    [Tooltip("신규 유저 로그인시 나타나게될 UI 순서")]
    [SerializeField] private List<StartClass> _newUserStartList;

    [SerializeField] private GameObject _uiStart;

    [SerializeField] private GameObject _mainScene;

    private StartClass _currentClass;

    private int _lastIndex;

    private int _nextIndex;

    private void Awake()
    {
        Init();
        _backgroundButton.onClick.AddListener(OnBackgroundButtonClickd);

    }

    private void Start()
    {
        if (! DatabaseManager.Instance.UserInfo.IsExistingUser)
        {
            _mainScene.SetActive(false);
            return;
        }
            
    }

    private void Update()
    {
            _currentClass?.UIUpdate();
    }


    private void Init()
    {
        //TODO: 차후 기존유저 로딩 생기면 풀기
        /*  if (!DatabaseManager.Instance.UserInfo.IsExistingUser)
          {
              _lastIndex = _newUserStartList.Count;
              for (int i = 0; i < _lastIndex; i++)
              {
                  _newUserStartList[i].Init(this);
              }
          }
          else
          {
              _lastIndex = _existingUserStartList.Count;
              for (int i = 0; i < _lastIndex; i++)
              {
                  _existingUserStartList[i].Init(this);
              }
          }*/

        _lastIndex = _newUserStartList.Count;
        for (int i = 0; i < _lastIndex; i++)
        {
            _newUserStartList[i].Init(this);
        }
        ChangeCurrentClass();
    }


    private void OnBackgroundButtonClickd()
    {
        _currentClass?.UIStart();
    }


    public void ChangeCurrentClass()
    {
        if (_nextIndex >= _lastIndex) //index를 모두 지나왔다면?
        {
            LoadingSceneManager.LoadScene("24_01_09_Integrated");
            return;
        }

        //TODO: 차후 기존유저 로딩 생기면 풀기
        /*  _currentClass = !DatabaseManager.Instance.UserInfo.IsExistingUser 
              ? _newUserStartList[_nextIndex] 
              : _currentClass = _existingUserStartList[_nextIndex];*/

        _currentClass = _newUserStartList[_nextIndex];

        _nextIndex++;
        OnBackgroundButtonClickd();
    }
}
