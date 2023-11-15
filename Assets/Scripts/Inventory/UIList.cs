using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//������ ����
public enum Field
{
    None = -1,
    Toy,
    Snack
}

public abstract class UIList<T> : MonoBehaviour where T : Item
{
    private Color[] _fieldColor; //panel ���� ������ ���� �迭
    [SerializeField] private GameObject _prefab; //spawn�� prefab
    [SerializeField] private Image _inventorySlots;//slots ���
    [SerializeField] private Button _closeDetailViewButton; //�󼼼��� â �ݱ�

    [SerializeField] protected ToggleGroup _field; //��� ����
    [SerializeField] protected GameObject _detailView; //�󼼼��� â
    [SerializeField] protected Transform[] _spawnPoint; //spawn�� ��ġ

    protected List<T>[] _lists = new List<T>[System.Enum.GetValues(typeof(Field)).Length-1]; //Field ������ŭ ����Ʈ ����(None ����) //�����͸� ������ ����
    protected Field _currentField;
    protected int[] _maxCount = new int[System.Enum.GetValues(typeof(Field)).Length - 1];

    /// <summary>
    /// Panel ���� �迭(_fieldColor)�� �����ϴ� �Լ�
    /// </summary>
    protected abstract Color[] SetFieldColorArray();

    /// <summary>
    /// DetailView Text �޾ƿ��� �Լ�
    /// </summary>
    protected abstract void GetContent(int index);

    /// <summary>
    /// InventorySlots�� SetActive(), Count() ui�� update�ϴ� �Լ�
    /// </summary>
    protected abstract void UpdateInventorySlots();

    void Awake()
    {
        _field.transform.GetChild(0).GetComponent<Toggle>().isOn = true; 
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

    //���� ���� �Լ�
    private void ChangeBackGroundColor()
    {
        Color[] fieldColor = SetFieldColorArray();

        Toggle selectedField = _field.ActiveToggles().FirstOrDefault(); //���õ� ���

        _currentField = GetFieldByTransform(selectedField);
        _inventorySlots.color = fieldColor[(int)_currentField];
    }

    //spawnPosition Ȱ��ȭ �Լ�
    private void SetActiveContent() 
    {
        for(int i=0;i<_spawnPoint.Length;i++) //content false�� ����
        {
            _spawnPoint[i].gameObject.SetActive(false);
        }
        _spawnPoint[(int)_currentField].gameObject.SetActive(true);//���� ����� content�� setactive 
    }

    //Transform���� Field�� ã��
    private Field GetFieldByTransform(Toggle toggle)
    {
        for(int i=0; i < _field.transform.childCount; i++)
        {
            if(_field.transform.GetChild(i).GetComponent<Toggle>() == toggle)
            {
                return (Field)i;
            }
        }
        return Field.None;
    }

    //SlotClick �̺�Ʈ
    private void OnClickSlot(int index)
    {
        GetContent(index); //�ؽ�Ʈ ���ε�
        _detailView.SetActive(true); //�� ���� â ��Ÿ��
    }

    private void OnClickFieldButton(bool isOn, Transform toggle)
    {
        if(isOn)
        {
            //mask active true
            toggle.GetChild(0).GetComponent<Mask>().enabled = true;

        }
        else
        {
            toggle.GetChild(0).GetComponent<Mask>().enabled = false;

        }

        SetActiveContent();
        ChangeBackGroundColor();
    }

    protected void Init()
    {
        CreateSlots(); //slot ����
        ChangeBackGroundColor(); //��� �� ����
        UpdateInventorySlots(); //�ʱ� slot update

        //��ư ������
        foreach (Toggle toggle in _field.GetComponentsInChildren<Toggle>()) //����� ����Ǹ� ��� ���� ��ȭ
        {
            toggle.onValueChanged.AddListener((bool isOn) => OnClickFieldButton(isOn, toggle.transform));
        }
        _closeDetailViewButton.onClick.AddListener(() => _detailView.SetActive(false));
    }
}
