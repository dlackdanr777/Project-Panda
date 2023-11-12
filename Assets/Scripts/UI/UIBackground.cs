using System.Data.Common;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIBackground : MonoBehaviour
{
    [SerializeField] private Color[] _fieldColor; //panel 색상 변경할 색상 배열

    private ToggleGroup _toggleGroup;

    private void Awake()
    {
        _toggleGroup = transform.GetChild(0).GetComponent<ToggleGroup>();
        
    }
    private void OnEnable()
    {
        Toggle firstToggle = _toggleGroup.transform.GetChild(0).GetComponent<Toggle>(); //다시 들어가도 첫번째가 활성화되도록
        if(firstToggle != null)
        {
            firstToggle.isOn = true;
        }
    }
    private void Start()
    {
        //버튼 리스너
        foreach (Toggle toggle in _toggleGroup.GetComponentsInChildren<Toggle>()) //토글이 변경되면 배경 색상도 변화
        {
            toggle.onValueChanged.AddListener((bool isOn) => OnClickedFieldButton(isOn, toggle.transform));
        }
        ChangeBackGroundColor(); //초기 배경 색
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

    //배경색 변경 함수
    private void ChangeBackGroundColor()
    {
        Toggle selectedField = GetOnToggle(); //선택된 토글
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

    //Transform으로 Field값 찾기
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
