using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataBinding : MonoBehaviour
{
    static Dictionary<string, TextData> _dataBindingText = null;

    static Dictionary<string, ButtonData> _dataBindingAction = null;

    /// <summary>
    /// 텍스트 데이터의 연결장소와 보낼 값을 지정해두는 함수(data값이 변할 경우 GetTextValue로 연결된 곳의 값도 변함)
    /// </summary>
    public static void SetTextValue(string dataID, string data)
    {
        if (_dataBindingText == null)
            _dataBindingText = new Dictionary<string, TextData>();

        if (!_dataBindingText.TryGetValue(dataID, out TextData textData))
        {
            textData = new TextData();
            _dataBindingText.Add(dataID, textData);
        }

        textData.text = data;
    }


    /// <summary>
    /// 저장된 텍스트 데이터의 연결장소를 불러와 연결하는 함수
    /// </summary>
    public static TextData GetTextValue(string dataID)
    {
        if (_dataBindingText == null)
            _dataBindingText = new Dictionary<string, TextData>();

        if (!_dataBindingText.TryGetValue(dataID, out TextData textData))
        {
            textData = new TextData();
            _dataBindingText.Add(dataID, textData);
        }
        return textData;
    }


    /// <summary>
    /// 버튼 데이터의 연결장소와 보낼값을 지정해두는 함수(action값이 변할 경우 GetButtonValue로 연결된 곳의 값도 변함)
    /// </summary>
    public static void SetButtonValue(string dataID, UnityAction action)
    {
        if(_dataBindingAction == null)
            _dataBindingAction = new Dictionary<string, ButtonData>();

        if (!_dataBindingAction.TryGetValue(dataID, out ButtonData buttonData))
        {
            buttonData = new ButtonData();
            _dataBindingAction.Add(dataID, buttonData);
        }
        buttonData.Action = action;
    }


    /// <summary>
    /// 버튼 텍스트 데이터의 연결장소를 불러오는 함수
    /// </summary>
    public static ButtonData GetButtonValue(string dataID)
    {
        if(_dataBindingAction == null)
            _dataBindingAction= new Dictionary<string, ButtonData>();

        if(!_dataBindingAction.TryGetValue(dataID, out ButtonData buttonData))
        {
            buttonData = new ButtonData();
            _dataBindingAction.Add(dataID, buttonData);
        }
        return buttonData;
    }

}
