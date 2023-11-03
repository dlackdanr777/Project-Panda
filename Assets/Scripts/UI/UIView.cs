using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UIView : MonoBehaviour
{
    public enum VisibleState
    {
        Disappeared, // 사라짐
        Disappearing, //사라지는 중
        Appeared, //나타남
        Appearing, //나타나는중
    }

    public VisibleState _visibleState;

    static Dictionary<string, TextData> _dataBindingTextToDic = null;

    /// <summary>
    /// 텍스트 데이터를 저장해두는 함수
    /// </summary>
    public static void SetValue(string dataID, string data)
    {
        if (_dataBindingTextToDic == null)
            _dataBindingTextToDic = new Dictionary<string, TextData>();

        if (!_dataBindingTextToDic.TryGetValue(dataID, out TextData textData))
        {
            textData = new TextData();
            _dataBindingTextToDic.Add(dataID, textData);
        }

        textData.text = data;
    }
    

    /// <summary>
    /// 저장된 데이터를 불러오는 함수
    /// </summary>
    public static TextData GetValue(string dataID)
    {
        if (_dataBindingTextToDic == null)
            _dataBindingTextToDic = new Dictionary<string, TextData>();

        if (!_dataBindingTextToDic.TryGetValue(dataID, out TextData textData))
        {
            textData = new TextData();
            _dataBindingTextToDic.Add(dataID, textData);
        }

        return textData;
    }

    /// <summary>
    /// UI를 불러낼때 콜백되는 함수
    /// </summary>
    public abstract void Show();


    /// <summary>
    /// UI를 끌때 콜백되는 함수
    /// </summary>
    public abstract void Hide();

}
