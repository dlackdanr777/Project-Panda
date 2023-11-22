using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIList<T, Enum> : MonoBehaviour 
{
    [SerializeField] private GameObject _prefab; //spawn�� prefab
    [SerializeField] private Button _closeDetailViewButton; //�󼼼��� â �ݱ�

    [SerializeField] protected ToggleGroup _field; //��� ����
    [SerializeField] protected GameObject _detailView; //�󼼼��� â
    [SerializeField] protected Transform[] _spawnPoint; //spawn�� ��ġ
    
    protected List<T>[] _lists = new List<T>[System.Enum.GetValues(typeof(Enum)).Length-1]; //Field ������ŭ ����Ʈ ����(None ����) //�����͸� ������ ����
    protected Enum _currentField;
    protected int[] _maxCount = new int[System.Enum.GetValues(typeof(Enum)).Length - 1];

    /// <summary>
    /// DetailView Text �޾ƿ��� �Լ�
    /// </summary>
    protected abstract void GetContent(int index);

    /// <summary>
    /// InventorySlots�� SetActive(), Count() ui�� update�ϴ� �Լ�
    /// </summary>
    protected abstract void UpdateListSlots();

    void OnEnable()
    {
        Toggle firstToggle = _field.transform.GetChild(0).GetComponent<Toggle>(); //�ٽ� ���� ù��°�� Ȱ��ȭ�ǵ���
        if (firstToggle != null)
        {
            firstToggle.isOn = true;
        }
        UpdateListSlots();
    }

    //list���� �ش� spawn ��ġ�� Instantiate�ϴ� �Լ�
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
        Toggle selectedField = GetOnToggle(); //���õ� ���
        _currentField = GetFieldByTransform(selectedField);
        SetActiveContent();
    }

    //spawnPosition Ȱ��ȭ �Լ�
    private void SetActiveContent() 
    {
        for(int i=0;i<_spawnPoint.Length;i++) //content false�� ����
        {
            _spawnPoint[i].gameObject.SetActive(false);
        }
        if((int)(object)_currentField >= 0)
        {
            _spawnPoint[(int)(object)_currentField].gameObject.SetActive(true);//���� ����� content�� setactive 

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

    //SlotClick �̺�Ʈ
    private void OnClickSlot(int index)
    {
        GetContent(index); //�ؽ�Ʈ ���ε�
        _detailView.SetActive(true); //�� ���� â ��Ÿ��
    }

    private void OnClickedFieldButton(bool isOn, Transform toggle)
    {
        GetCurrentField();
        if ((int)(object)_currentField != -1)
        {
            UpdateListSlots();  
        }

    }

    //Transform���� Field�� ã��
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
        if (_detailView.activeSelf)
        {
            _detailView.SetActive(false);

        }
    }

    

    protected void Init()
    {
        CreateSlots(); //slot ����

        //��ư ������
        foreach (Toggle toggle in _field.GetComponentsInChildren<Toggle>()) //����� ����Ǹ� ��� ���� ��ȭ
        {
            toggle.onValueChanged.AddListener((bool isOn) => OnClickedFieldButton(isOn, toggle.transform));
        }
        _closeDetailViewButton.onClick.AddListener(() => _detailView.SetActive(false));
    }
}
