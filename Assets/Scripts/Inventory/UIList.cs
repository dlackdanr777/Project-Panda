using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//아이템 종류
public enum Field
{
    None = -1,
    Toy,
    Snack
}

public abstract class UIList<T> : MonoBehaviour where T : Item
{
    [SerializeField] private GameObject _prefab; //spawn할 prefab
    [SerializeField] private Image _inventorySlots;//slots 배경
    [SerializeField] private Button _closeDetailViewButton; //상세설명 창 닫기

    [SerializeField] protected ToggleGroup _field; //토글 종류
    [SerializeField] protected GameObject _detailView; //상세설명 창
    [SerializeField] protected Transform[] _spawnPoint; //spawn할 위치

    protected List<T>[] _lists = new List<T>[System.Enum.GetValues(typeof(Field)).Length-1]; //Field 개수만큼 리스트 존재(None 제외) //데이터를 저장할 공간
    protected Field _currentField;
    protected Color[] _fieldColor; //panel 색상 변경할 색상 배열
    protected int[] _maxCount = new int[System.Enum.GetValues(typeof(Field)).Length - 1];

    /// <summary>
    /// Panel 색상 배열(_fieldColor)을 설정하는 함수
    /// </summary>
    protected abstract void SetFieldColorArray();

    /// <summary>
    /// DetailView Text 받아오는 함수
    /// </summary>
    protected abstract void GetContent(int index);

    /// <summary>
    /// InventorySlots의 SetActive(), Count() ui를 update하는 함수
    /// </summary>
    protected abstract void UpdateInventorySlots();

    protected void Init()
    {
        CreateSlots(); //slot 생성
        ChangeBackGroundColor(); //배경 색 변경
        UpdateInventorySlots(); //초기 slot update
        
        //버튼 리스너
        foreach (Toggle toggle in _field.GetComponentsInChildren<Toggle>()) //토글이 변경되면 배경 색상도 변화
        {
            toggle.onValueChanged.AddListener(
                (bool isOn) =>
                {
                    SetActiveContent();
                    ChangeBackGroundColor();
                });
        }
        _closeDetailViewButton.onClick.AddListener(() => _detailView.SetActive(false));
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

    //배경색 변경 함수
    private void ChangeBackGroundColor() 
    {
        _fieldColor = new Color[_field.transform.childCount];
        SetFieldColorArray();

        Toggle selectedField = _field.ActiveToggles().FirstOrDefault(); //선택된 토글

        _currentField = GetIndexByTransform(selectedField.transform);
        _inventorySlots.color = _fieldColor[(int)_currentField];
    }

    //spawnPosition 활성화 함수
    private void SetActiveContent() 
    {
        for(int i=0;i<_spawnPoint.Length;i++) //content false로 변경
        {
            _spawnPoint[i].gameObject.SetActive(false);
        }
        _spawnPoint[(int)_currentField].gameObject.SetActive(true);//현재 토글의 content를 setactive 
    }

    //Transform으로 Field값 찾기
    private Field GetIndexByTransform(Transform transform)
    {
        for(int i=0; i < _field.transform.childCount; i++)
        {
            if(_field.transform.GetChild(i) == transform.transform)
            {
                return (Field)i;
            }
        }
        return Field.None;
    }

    //SlotClick 이벤트
    private void OnClickSlot(int index)
    {
        GetContent(index); //텍스트 바인딩
        _detailView.SetActive(true); //상세 설명 창 나타남
    }
}
