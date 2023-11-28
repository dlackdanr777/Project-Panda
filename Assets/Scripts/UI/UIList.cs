using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIList<T, Enum> : MonoBehaviour 
{
    [SerializeField] private GameObject _prefab; //spawn할 prefab
    [SerializeField] private Button _closeDetailViewButton; //상세설명 창 닫기
    [SerializeField] private int _firstToggleIndex;

    [SerializeField] protected ToggleGroup _field; //토글 종류
    [SerializeField] protected GameObject _detailView; //상세설명 창
    [SerializeField] protected Transform[] _spawnPoint; //spawn할 위치
    
    protected List<T>[] _lists = new List<T>[System.Enum.GetValues(typeof(Enum)).Length-1]; //Field 개수만큼 리스트 존재(None 제외) //데이터를 저장할 공간
    protected Enum _currentField;
    protected int[] _maxCount = new int[System.Enum.GetValues(typeof(Enum)).Length - 1];

    /// <summary>
    /// DetailView Text 받아오는 함수
    /// </summary>
    protected abstract void GetContent(int index);

    /// <summary>
    /// InventorySlots의 SetActive(), Count() ui를 update하는 함수
    /// </summary>
    protected abstract void UpdateListSlots();

    void OnEnable()
    {
        Toggle firstToggle = _field.transform.GetChild(_firstToggleIndex).GetComponent<Toggle>(); //다시 들어가도 첫번째가 활성화되도록
        if (firstToggle != null)
        {
            firstToggle.isOn = true;
        }
        UpdateListSlots();
    }

    //list마다 해당 spawn 위치에 Instantiate하는 함수
    private void CreateSlots()
    {
        for(int i=0;i<_maxCount.Length;i++)
        {
            for (int j = 0; j < _maxCount[i]; j++)
            {
                GameObject slot = Instantiate(_prefab, _spawnPoint[i]);
                int _slotIndex = j;
                slot.GetComponent<Button>().onClick.AddListener(()=>OnClickSlot(_slotIndex));
            }
        }
    }

    private void GetCurrentField()
    {
        Toggle selectedField = GetOnToggle(); //선택된 토글
        _currentField = GetFieldByTransform(selectedField);
        SetActiveContent();
    }

    //spawnPosition 활성화 함수
    private void SetActiveContent() 
    {
        for(int i=0;i<_spawnPoint.Length;i++) //content false로 변경
        {
            _spawnPoint[i].gameObject.SetActive(false);
        }
        if((int)(object)_currentField >= 0)
        {
            _spawnPoint[(int)(object)_currentField].gameObject.SetActive(true);//현재 토글의 content를 setactive 

        }
    }

    private Toggle GetOnToggle()
    {
        foreach (Toggle toggle in _field.transform.GetComponentsInChildren<Toggle>())
        {
            if (toggle.isOn == true)
            {
                return toggle;
            }
        }
        return null;
    }

    //SlotClick 이벤트
    private void OnClickSlot(int index)
    {
        GetContent(index); //텍스트 바인딩
        if(_detailView != null)
        {
            _detailView.SetActive(true); //상세 설명 창 나타남

        }
    }

    private void OnClickedFieldButton(bool isOn, Transform toggle)
    {
        GetCurrentField();
        if ((int)(object)_currentField != -1)
        {
            UpdateListSlots();  
        }

    }

    //Transform으로 Field값 찾기
    private Enum GetFieldByTransform(Toggle toggle)
    {
        for(int i=0; i < _field.transform.childCount; i++)
        {
            if(_field.transform.GetChild(i).GetComponent<Toggle>() == toggle)
            {
                return (Enum)(object)i;
            }
        }
        return (Enum)(object)-1;
    }

    protected virtual void OnDisable()
    {
        if (_detailView != null && _detailView.activeSelf)
        {
            _detailView.SetActive(false);

        }
    }

    

    protected void Init()
    {
        CreateSlots(); //slot 생성

        //버튼 리스너
        foreach (Toggle toggle in _field.GetComponentsInChildren<Toggle>()) //토글이 변경되면 배경 색상도 변화
        {
            toggle.onValueChanged.AddListener((bool isOn) => OnClickedFieldButton(isOn, toggle.transform));
        }

        if(_detailView != null)
        {
            _closeDetailViewButton.onClick.AddListener(() => _detailView.SetActive(false));

        }
    }
}