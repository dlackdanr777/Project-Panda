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
    [SerializeField] private GameObject _prefab; //spawn�� prefab
    [SerializeField] private Image _inventorySlots;//slots ���
    [SerializeField] private Button _closeDetailViewButton; //�󼼼��� â �ݱ�

    [SerializeField] protected ToggleGroup _field; //��� ����
    [SerializeField] protected GameObject _detailView; //�󼼼��� â
    [SerializeField] protected Transform[] _spawnPoint; //spawn�� ��ġ

    protected List<T>[] _lists = new List<T>[System.Enum.GetValues(typeof(Field)).Length-1]; //Field ������ŭ ����Ʈ ����(None ����) //�����͸� ������ ����
    protected Field _currentField;
    protected Color[] _fieldColor; //panel ���� ������ ���� �迭
    protected int[] _maxCount = new int[System.Enum.GetValues(typeof(Field)).Length - 1];

    /// <summary>
    /// Panel ���� �迭(_fieldColor)�� �����ϴ� �Լ�
    /// </summary>
    protected abstract void SetFieldColorArray();

    /// <summary>
    /// DetailView Text �޾ƿ��� �Լ�
    /// </summary>
    protected abstract void GetContent(int index);

    /// <summary>
    /// InventorySlots�� SetActive(), Count() ui�� update�ϴ� �Լ�
    /// </summary>
    protected abstract void UpdateInventorySlots();

    protected void Init()
    {
        CreateSlots(); //slot ����
        ChangeBackGroundColor(); //��� �� ����
        UpdateInventorySlots(); //�ʱ� slot update
        
        //��ư ������
        foreach (Toggle toggle in _field.GetComponentsInChildren<Toggle>()) //����� ����Ǹ� ��� ���� ��ȭ
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
        _fieldColor = new Color[_field.transform.childCount];
        SetFieldColorArray();

        Toggle selectedField = _field.ActiveToggles().FirstOrDefault(); //���õ� ���

        _currentField = GetIndexByTransform(selectedField.transform);
        _inventorySlots.color = _fieldColor[(int)_currentField];
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

    //SlotClick �̺�Ʈ
    private void OnClickSlot(int index)
    {
        GetContent(index); //�ؽ�Ʈ ���ε�
        _detailView.SetActive(true); //�� ���� â ��Ÿ��
    }
}
