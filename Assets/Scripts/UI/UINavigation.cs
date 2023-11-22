using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct ViewDicStruct
{
    [Tooltip("ViewŬ������ �̸�")]
    public string Name;
    public UIView UIView;
}



public class UINavigation : MonoBehaviour
{
    [Tooltip("�� Ŭ�������� ������ UIView�� �ִ� ��")]
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
    /// �̸��� �޾� ���� �̸��� view�� �����ִ� �Լ�
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
                Debug.Log("�̹� ���ÿ� �����ϴ� uiŬ���� �Դϴ�.");
            }

        }
        else
        {
            Debug.LogError("��ųʸ��� �ش� �̸��� ���� UIViewŬ������ �����ϴ�.");
        }
    }


    /// <summary>
    /// ���� ui ���� ���ȴ� ui�� �ҷ����� �Լ�
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
    /// �� ó�� ���ȴ� ui�� �̵��ϴ� �Լ�
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
            Debug.LogError("�����ִ� ui�� �������� �ʽ��ϴ�.");
        }
    }


    /// <summary>
    /// ��� ui�� �޴� �Լ�
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
