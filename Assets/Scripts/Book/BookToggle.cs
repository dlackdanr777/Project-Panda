using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BookToggle : MonoBehaviour
{
    private ToggleGroup _toggleGroup;
    private Dictionary<int, Toggle> _toggleList; //å ��� <����, �ش� ���>

    //toggle�� ������ �����ٰ� �ٽ� ���ƿ� => ������ �������� ����
    void Start()
    {
        _toggleGroup = GetComponent<ToggleGroup>();
        _toggleList = new Dictionary<int, Toggle>();   

        //���� ��� ����
        foreach(var toggle in _toggleGroup.GetComponentsInChildren<Toggle>().Select((value, index) => (value, index))) //Select : index�� toggle �� �� �̿��ϱ� ���� 
        {
            Toggle t = toggle.value;
            toggle.value.onValueChanged.AddListener((isOn) => OnClickToggle(isOn, t));

            _toggleList.Add(toggle.index, toggle.value);
        }
    }

    private void OnClickToggle(bool isOn, Toggle t)
    {
        //ToggleGroup�� ������ �ִ� ��� Toggle ����
        foreach(var toggle in _toggleList)
        {
            toggle.Value.transform.SetParent(transform); //���� ��ġ�� �θ��
            toggle.Value.transform.SetSiblingIndex(toggle.Key); //���� ������
        }
        
        t.transform.SetParent(transform.parent); 
        t.transform.SetAsLastSibling();
    }
}
