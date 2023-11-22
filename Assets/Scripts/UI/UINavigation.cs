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
    [Tooltip("이 클래스에서 관리할 UIView를 넣는 곳")]
    [SerializeField] private ViewDicStruct[] _uiViewList;

    [SerializeField] private UIView _currentView;

    private Stack<UIView> _uiViews;

    private readonly Dictionary<string, UIView> _viewDic = new Dictionary<string, UIView>();

    public int Count => _uiViews.Count;


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _uiViews = new Stack<UIView>();

        for(int i = 0, count = _uiViewList.Length; i < count; i++)
        {
            string Name = _uiViewList[i].Name;
            UIView _uiView = _uiViewList[i].UIView;

            _viewDic.Add(Name, _uiView);

            _uiView.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// 이름을 받아 현재 이름의 view를 열어주는 함수
    /// </summary>
    public void Push(string viewName)
    {

        if(_viewDic.TryGetValue(viewName, out var r))
        {

        }

        if (_viewDic.ContainsKey(viewName))
        {
            if (!_uiViews.Contains(_viewDic[viewName]))
            {
                if (_currentView != null)
                {
                    _currentView.Hide();
                    _uiViews.Push(_currentView);
                }
                UIView view = _viewDic[viewName];
                _currentView = view;
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
        if (_uiViews.Count > 0)
        {
            _currentView.Hide();
            _currentView = _uiViews.Pop();
            _currentView.Show();
        }

        if (_currentView != null)
        {
            _currentView.Hide();
            _currentView = null;
        }

    }


    /// <summary>
    /// 맨 처음 열렸던 ui로 이동하는 함수
    /// </summary>
   public void PopToLoot()
    {

        if (_uiViews.Count > 0)
        {
            while (_uiViews.Count > 1)
            {
                _uiViews.Pop();
            }

            for (int i = 0, count = _uiViews.Count - 1; i < count; i++)
            {
                _uiViews.Pop();
            }
            _currentView.Hide();
            _currentView = _uiViews.Pop();

            Pop();
        }
        else
        {
            Debug.LogError("켜져있는 ui가 존재하지 않습니다.");
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

        if (_currentView!=null)
        {
            _currentView.Hide();
            _currentView = null;
        }

        _currentView?.Hide();
        _currentView = null;
    }
}
