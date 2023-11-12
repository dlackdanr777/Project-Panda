using System.Data.Common;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIBackground : MonoBehaviour
{
    [SerializeField] private Color[] _fieldColor; //panel ���� ������ ���� �迭

    private ToggleGroup _toggleGroup;

    private void Awake()
    {
        _toggleGroup = transform.GetChild(0).GetComponent<ToggleGroup>();
        
    }
    private void OnEnable()
    {
        Toggle firstToggle = _toggleGroup.transform.GetChild(0).GetComponent<Toggle>(); //�ٽ� ���� ù��°�� Ȱ��ȭ�ǵ���
        if(firstToggle != null)
        {
            firstToggle.isOn = true;
        }
    }
    private void Start()
    {
        //��ư ������
        foreach (Toggle toggle in _toggleGroup.GetComponentsInChildren<Toggle>()) //����� ����Ǹ� ��� ���� ��ȭ
        {
            toggle.onValueChanged.AddListener((bool isOn) => OnClickedFieldButton(isOn, toggle.transform));
        }
        ChangeBackGroundColor(); //�ʱ� ��� ��
    }

    private void OnDisable()
    {
        foreach(Toggle toggle in _toggleGroup.transform.GetComponentsInChildren<Toggle>())
        {
            toggle.isOn = false;
        }
    }

    private void OnClickedFieldButton(bool isOn, Transform toggle)
    {
        toggle.GetChild(0).GetComponent<Mask>().enabled = isOn;
        ChangeBackGroundColor();
    }

    //���� ���� �Լ�
    private void ChangeBackGroundColor()
    {
        Toggle selectedField = GetOnToggle(); //���õ� ���
        int currentIndex = GetIndexByTransform(selectedField);
        if(currentIndex >= 0)
        {
            GetComponent<Image>().color = _fieldColor[currentIndex];

        }
    }

    private Toggle GetOnToggle()
    {
        foreach(Toggle toggle in _toggleGroup.transform.GetComponentsInChildren<Toggle>())
        {
            if(toggle.isOn == true)
            {
                return toggle;
            }
        }
        return null;
    }

    //Transform���� Field�� ã��
    private int GetIndexByTransform(Toggle toggle)
    {
        for (int i = 0; i < _toggleGroup.transform.childCount; i++)
        {
            if (_toggleGroup.transform.GetChild(i).GetComponent<Toggle>() == toggle)
            {
                return i;
            }
        }
        return -1;
    }
}
