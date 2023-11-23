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

    [Tooltip("�ֻ��� lootUIView�� �ִ� ��")]
    [SerializeField] private ViewDicStruct _rootUiView;

    [Tooltip("�� Ŭ�������� ������ UIView�� �ִ� ��")]
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
    /// �̸��� �޾� ���� �̸��� view�� �����ִ� �Լ�
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
        if (_uiViews.Count <= 0)
            return;

        _currentView.Hide();
        _currentView = _uiViews.Pop();
        _currentView.Show();
    }


    /// <summary>
    /// �� ó�� ���ȴ� ui�� �̵��ϴ� �Լ�
    /// </summary>
   public void PopToLoot()
    {
        while(_uiViews.Count > 1)
        {
            _uiViews.Pop();
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
    }
}
