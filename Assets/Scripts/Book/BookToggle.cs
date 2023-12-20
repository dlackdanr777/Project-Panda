using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BookToggle : MonoBehaviour
{
    public static Action<int> OnMoveBookPage = delegate { };
    [SerializeField] GameObject[] _pages;

    private ToggleGroup _toggleGroup;
    private Dictionary<int, Toggle> _toggleList; //å ��� <����, �ش� ���>

    //toggle�� ������ �����ٰ� �ٽ� ���ƿ� => ������ �������� ����
    void Awake()
    {
        _toggleGroup = GetComponent<ToggleGroup>();
        _toggleList = new Dictionary<int, Toggle>();   

        //���� ��� ����
        foreach(var toggle in _toggleGroup.GetComponentsInChildren<Toggle>().Select((value, index) => (value, index))) //Select : index�� toggle �� �� �̿��ϱ� ���� 
        {
            Toggle t = toggle.value;
            toggle.value.onValueChanged.AddListener((isOn) => OnClickToggle(isOn, t, toggle.index));

            _toggleList.Add(toggle.index, toggle.value);
        }


    }

    private void OnEnable() //ù ��° ��� ����
    {
        _toggleList[0].isOn = true;
        OnClickToggle(true, _toggleList[0], 0);
    }

    private void OnClickToggle(bool isOn, Toggle t, int index)
    {
        //ToggleGroup�� ������ �ִ� ��� Toggle ����
        foreach(var toggle in _toggleList)
        {
            toggle.Value.transform.SetParent(transform); //���� ��ġ�� �θ��
            toggle.Value.transform.SetSiblingIndex(toggle.Key); //���� ������
        }
        
        t.transform.SetParent(transform.parent); 
        t.transform.SetAsLastSibling();

        //Book page �̵�
        for (int i = 0; i < _toggleList.Count; i++)
        {
            if (i == index)
            {
                _pages[i].SetActive(true);
            }
            else
            {
                _pages[i].SetActive(false);
            }
        }
        //OnMoveBookPage?.Invoke(index);
    }
}
