using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct ViewDicStruct
{
    [Tooltip("View클래스의 이름")]
    public string Name;
    public UIView UIView;
}

public class UINavigation : MonoBehaviour
{

    [Tooltip("최상위 lootUIView를 넣는 곳")]
    [SerializeField] private ViewDicStruct _rootUiView;

    [Tooltip("이 클래스에서 관리할 UIView를 넣는 곳")]
    [SerializeField] private ViewDicStruct[] _uiViewList;

    [SerializeField] private UIView _currentView;

    private Stack<UIView> _uiViews = new Stack<UIView>();

    private Dictionary<string, UIView> _viewDic = new Dictionary<string, UIView>();

    public int Count => _uiViews.Count;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _viewDic.Add(_rootUiView.Name, _rootUiView.UIView);
        _currentView = _rootUiView.UIView;
        _rootUiView.UIView.Init(this);

        for(int i = 0, count = _uiViewList.Length; i < count; i++)
        {
            string name = _uiViewList[i].Name;
            UIView uiView = _uiViewList[i].UIView;

            _viewDic.Add(name, uiView);
            uiView.Init(this);
            uiView.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// 이름을 받아 현재 이름의 view를 열어주는 함수
    /// </summary>
    public void Push(string viewName)
    {
        if (_viewDic.TryGetValue(viewName, out UIView uiView))
        {
            if (!_uiViews.Contains(uiView))
            {
                if(_currentView != null)
                {
                    _uiViews.Push(_currentView);
                    _currentView.Hide();
                }
                _currentView = uiView;
                _currentView.Show();
            }
            else
            {
                Debug.Log("이미 스택에 존재하는 ui클래스 입니다.");
            }
        }
        else
        {
            Debug.LogError("딕셔너리에 해당 이름을 가진 UIView클래스가 없습니다.");
        }
    }


    /// <summary>
    /// 현재 ui 전에 열렸던 ui를 불러오는 함수
    /// </summary>
    public void Pop()
    {
        if (_uiViews.Count <= 0)
            return;

        _currentView.Hide();
        _currentView = _uiViews.Pop();
        _currentView.Show();
    }


    /// <summary>
    /// 맨 처음 열렸던 ui로 이동하는 함수
    /// </summary>
   public void PopToLoot()
    {
        while(_uiViews.Count > 1)
        {
            _uiViews.Pop();
        }
    }


    /// <summary>
    /// 모든 ui를 받는 함수
    /// </summary>
    public void Clear()
    {
        for (int i = 0, count = _uiViews.Count; i < count; i++)
        {
            _uiViews.Pop();
        }
    }
}
