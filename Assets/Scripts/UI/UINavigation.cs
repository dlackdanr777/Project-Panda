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

    private Stack<UIView> _uiViews;

    private UIView _currentView;

    private Dictionary<string, UIView> _viewDic = new Dictionary<string, UIView>();

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Push("Album");
        }
    }

    private void Init()
    {
        _uiViews = new Stack<UIView>();
        for(int i = 0, count = _uiViewList.Length; i < count; i++)
        {
            string Name = _uiViewList[i].Name;
            UIView _uiView = _uiViewList[i].UIView;

            _viewDic.Add(Name, _uiView);
        }
    }


    /// <summary>
    /// �̸��� �޾� ���� �̸��� view�� �����ִ� �Լ�
    /// </summary>
    public void Push(string viewName)
    {
        if (_viewDic.ContainsKey(viewName))
        {
            if(_currentView != null)
                _currentView.Hide();

            UIView view = _viewDic[viewName];
            _uiViews.Push(view);
            _currentView = view;
            _currentView.Show();
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
        _currentView.Hide();

        if (_uiViews.Count > 0)
        {
            _currentView = _uiViews.Pop();
            _currentView.Show();
        }
    }

    /// <summary>
    /// �� ó�� ���ȴ� ui�� �̵��ϴ� �Լ�
    /// </summary>
   public void PopToLoot()
    {
        if (_uiViews.Count > 0)
        {
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
}
