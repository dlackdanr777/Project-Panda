using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIBackground : MonoBehaviour
{
    [SerializeField] private Color[] _fieldColor; //panel 색상 변경할 색상 배열

    private ToggleGroup _toggleGroup;

    private void Start()
    {
        _toggleGroup = transform.GetChild(0).GetComponent<ToggleGroup>();
        //버튼 리스너
        foreach (Toggle toggle in _toggleGroup.GetComponentsInChildren<Toggle>()) //토글이 변경되면 배경 색상도 변화
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

    //배경색 변경 함수
    private void ChangeBackGroundColor()
    {
        Toggle selectedField = _toggleGroup.ActiveToggles().FirstOrDefault(); //선택된 토글
        int currentIndex = GetIndexByTransform(selectedField);
        GetComponent<Image>().color = _fieldColor[currentIndex];
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
