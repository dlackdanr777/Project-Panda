using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private List<UIView> _uiViews = new List<UIView>();

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


    /// <summary>
    /// 이름을 받아 현재 이름의 view를 열어주는 함수
    /// </summary>
    public void Push(string viewName)
    {

            
        if (_viewDic.TryGetValue(viewName, out UIView uiView))
        {
            if(Count > 0)
            {
                if (_uiViews.Last().VisibleState == VisibleState.Disappearing || _uiViews.Last().VisibleState == VisibleState.Appearing)
                {
                    Debug.Log("UI가 열리거나 닫히는 중 입니다.");
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
            
            uiView.RectTransform.SetAsLastSibling();
            CheckViewListCount();
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
        if (Count > 0)
        {
            if (_uiViews.Last().VisibleState == VisibleState.Disappearing || _uiViews.Last().VisibleState == VisibleState.Appearing)
            {
                Debug.Log("UI가 열리거나 닫히는 중 입니다.");
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
    /// viewName을 확인해 해당 UI 를 감추는 함수
    /// </summary>
    public void Pop(string viewName)
    {
        if (Count > 0)
        {
            if (_uiViews.Last().VisibleState == VisibleState.Disappearing || _uiViews.Last().VisibleState == VisibleState.Appearing)
            {
                Debug.Log("UI가 열리거나 닫히는 중 입니다.");
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
    /// 맨 처음 열렸던 ui로 이동하는 함수
    /// </summary>
    public void Clear()
    {
        if (Count > 0)
        {
            if (_uiViews.Last().VisibleState == VisibleState.Disappearing || _uiViews.Last().VisibleState == VisibleState.Appearing)
            {
                Debug.Log("UI가 열리거나 닫히는 중 입니다.");
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
