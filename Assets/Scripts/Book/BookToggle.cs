using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BookToggle : MonoBehaviour
{
    private ToggleGroup _toggleGroup;
    private Dictionary<int, Toggle> _toggleList; //책 토글 <순서, 해당 토글>

    //toggle이 밖으로 나갔다가 다시 돌아옴 => 앞으로 보여지기 위함
    void Start()
    {
        _toggleGroup = GetComponent<ToggleGroup>();
        _toggleList = new Dictionary<int, Toggle>();   

        //현재 토글 저장
        foreach(var toggle in _toggleGroup.GetComponentsInChildren<Toggle>().Select((value, index) => (value, index))) //Select : index와 toggle 둘 다 이용하기 위해 
        {
            Toggle t = toggle.value;
            toggle.value.onValueChanged.AddListener((isOn) => OnClickToggle(isOn, t));

            _toggleList.Add(toggle.index, toggle.value);
        }
    }

    private void OnClickToggle(bool isOn, Toggle t)
    {
        //ToggleGroup이 가지고 있는 모든 Toggle 구함
        foreach(var toggle in _toggleList)
        {
            toggle.Value.transform.SetParent(transform); //현재 위치를 부모로
            toggle.Value.transform.SetSiblingIndex(toggle.Key); //순서 재정렬
        }
        
        t.transform.SetParent(transform.parent); 
        t.transform.SetAsLastSibling();
    }
}
