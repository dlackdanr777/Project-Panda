using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private Transform[] _spawnPoint; //spawn�� ��ġ
    [SerializeField] private ToggleGroup _field; //��� ����
    [SerializeField] private Image _inventorySlots;//slots ���
    [SerializeField] private GameObject _detailView; //�󼼼��� â
    [SerializeField] private Button _closeDetailViewButton; //�󼼼��� â �ݱ�

    protected List<T>[] _lists = new List<T>[System.Enum.GetValues(typeof(Field)).Length-1]; //Field ������ŭ ����Ʈ ����(None ����) //�����͸� ������ ����
    protected Field _currentField;
    protected List<T> _list;
    protected Color[] _fieldColor; //panel ���� ������ ���� �迭

    /// <summary>
    /// Panel ���� �迭(_fieldColor)�� �����ϴ� �Լ�
    /// </summary>
    public abstract void SetFieldColorArray();

    protected void Init()
    {
        _currentField = Field.Toy; //ó���� ���õ� �峭������ �ʱ�ȭ

        ChangeBackGroundColor(); //��� �� ����


        //��ư ������
        foreach(Toggle toggle in _field.GetComponentsInChildren<Toggle>()) //����� ����Ǹ� ��� ���� ��ȭ
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
    
    //list���� �ش� spawn ��ġ�� Instantiate
    protected void CreateSlots()
    {
        _list = _lists[(int)_currentField];

        for (int i = 0; i < _list.Count; i++)
        {
            GameObject slot = Instantiate(_prefab, _spawnPoint[(int)_currentField]);
            int index = i;
            slot.GetComponent<Button>().onClick.AddListener(()=>OnClickSlot(index));
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
        _detailView.SetActive(true); //�� ���� â ��Ÿ��
        //����� text ��¼���� �� ���� �޾ƿ�
    }
}
