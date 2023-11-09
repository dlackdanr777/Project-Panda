using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIBackground : MonoBehaviour
{
    [SerializeField] private Color[] _fieldColor; //panel ���� ������ ���� �迭

    private ToggleGroup _toggleGroup;

    private void Start()
    {
        _toggleGroup = transform.GetChild(0).GetComponent<ToggleGroup>();
        //��ư ������
        foreach (Toggle toggle in _toggleGroup.GetComponentsInChildren<Toggle>()) //����� ����Ǹ� ��� ���� ��ȭ
        {
            toggle.onValueChanged.AddListener((bool isOn) => OnClickedFieldButton(isOn, toggle.transform));
        }

        ChangeBackGroundColor();
    }

    private void OnClickedFieldButton(bool isOn, Transform toggle)
    {
        if (isOn)
        {
            //mask active true
            toggle.GetChild(0).GetComponent<Mask>().enabled = true;

        }
        else
        {
            toggle.GetChild(0).GetComponent<Mask>().enabled = false;

        }
        ChangeBackGroundColor();
    }

    //���� ���� �Լ�
    private void ChangeBackGroundColor()
    {
        Toggle selectedField = _toggleGroup.ActiveToggles().FirstOrDefault(); //���õ� ���
        int currentIndex = GetIndexByTransform(selectedField);
        GetComponent<Image>().color = _fieldColor[currentIndex];
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
