using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]  private List<UIView> _uiViews = new List<UIView>();

    private Dictionary<string, UIView> _viewDic = new Dictionary<string, UIView>();

    public int Count => _uiViews.Count;



    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pop();
        }
    }

    private void Init()
    {
        _rootUiView.UIView.Init(this);

        for (int i = 0, count = _uiViewList.Length; i < count; i++)
        {
            string name = _uiViewList[i].Name;
            UIView uiView = _uiViewList[i].UIView;
            _viewDic.Add(name, uiView);
            uiView.Init(this);
            uiView.gameObject.SetActive(false);
        }
    }

    public bool Check(string viewName)
    {
        if (_viewDic.TryGetValue(viewName, out UIView uiView))
        {
            if(_uiViews.Contains(uiView))
                return true;
        }
        return false;

    }


    /// <summary>
    /// �̸��� �޾� ���� �̸��� view�� �����ִ� �Լ�
    /// </summary>
    public void Push(string viewName)
    {
        if (_viewDic.TryGetValue(viewName, out UIView uiView))
        {

            foreach (UIView view in _viewDic.Values)
            {
                if (view.VisibleState == VisibleState.Disappearing || view.VisibleState == VisibleState.Appearing)
                {
                    Debug.Log("UI�� �����ų� ������ �� �Դϴ�.");
                    return;
                }
            }

            if (!_uiViews.Contains(uiView))
            {
                _uiViews.Add(uiView);
                uiView.Show();
            }
            else
            {
                _uiViews.Remove(uiView);
                _uiViews.Add(uiView);
            }

            uiView.gameObject.SetActive(true);
            uiView.RectTransform.SetAsLastSibling();
            CheckViewListCount();
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

        foreach (UIView view in _viewDic.Values)
        {
            if (view.VisibleState == VisibleState.Disappearing || view.VisibleState == VisibleState.Appearing)
            {
                Debug.Log("UI�� �����ų� ������ �� �Դϴ�.");
                return;
            }
        }         

        if (_uiViews.Count <= 0)
            return;

        //_currentView.Hide();
        _uiViews.Last().Hide();
        _uiViews.RemoveAt(Count - 1);

        if (1 <= _uiViews.Count)
            _uiViews.Last().RectTransform.SetAsLastSibling();

        CheckViewListCount();

    }


    /// <summary>
    /// viewName�� Ȯ���� �ش� UI �� ���ߴ� �Լ�
    /// </summary>
    public void Pop(string viewName)
    {

        foreach (UIView view in _viewDic.Values)
        {
            if (view.VisibleState == VisibleState.Disappearing || view.VisibleState == VisibleState.Appearing)
            {
                Debug.Log("UI�� �����ų� ������ �� �Դϴ�.");
                return;
            }
        }

        if (_uiViews.Count <= 0)
            return;

        if (_uiViews.Find(x => x == _viewDic[viewName]) == null)
            return;

        UIView selectView = _uiViews.Find(x => x == _viewDic[viewName]);
        selectView.Hide();
        _uiViews.Remove(selectView);

        if (1 <= _uiViews.Count)
            _uiViews.Last().RectTransform.SetAsLastSibling();

        CheckViewListCount();
    }


    /// <summary>
    /// �� ó�� ���ȴ� ui�� �̵��ϴ� �Լ�
    /// </summary>
    public void Clear()
    {

        foreach (UIView view in _viewDic.Values)
        {
            if (view.VisibleState == VisibleState.Disappearing || view.VisibleState == VisibleState.Appearing)
            {
                Debug.Log("UI�� �����ų� ������ �� �Դϴ�.");
                return;
            }
        }


        while (_uiViews.Count > 0)
        {
            _uiViews.Last().Hide();
            _uiViews.Remove(_uiViews.Last());
        }
        CheckViewListCount();
    }


    /// <summary> ������ ��� UIView�� SetActive(true)�Ѵ�. </summary>
    public void AllShow()
    {
        _rootUiView.UIView.gameObject.SetActive(true);

        foreach (UIView view in _uiViews)
        {
            view.gameObject.SetActive(true);
        }
    }


    /// <summary> �ѳ��� ��� UIView�� SetActive(false)�Ѵ�. </summary>
    public void AllHide()
    {
        _rootUiView.UIView.gameObject.SetActive(false);

        foreach(UIView view in _uiViews)
        {
            view.gameObject.SetActive(false);
        }
    }


    public void CheckViewListCount()
    {
        if(0 < _uiViews.Count)
        {
            GameManager.Instance.FriezeCameraMove = true;
            GameManager.Instance.FriezeCameraZoom = true;
            GameManager.Instance.FirezeInteraction = true;
        }
        else
        {
            GameManager.Instance.FriezeCameraMove = false;
            GameManager.Instance.FriezeCameraZoom = false;
            GameManager.Instance.FirezeInteraction = false;
        }
    }
}
