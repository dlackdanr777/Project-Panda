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
    /// �ؽ�Ʈ �������� ������ҿ� ���� ���� �����صδ� �Լ�(data���� ���� ��� GetTextValue�� ����� ���� ���� ����)
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
    /// ����� �ؽ�Ʈ �������� ������Ҹ� �ҷ��� �����ϴ� �Լ�
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
    /// ��ư �������� ������ҿ� �������� �����صδ� �Լ�(action���� ���� ��� GetButtonValue�� ����� ���� ���� ����)
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
    /// ��ư �ؽ�Ʈ �������� ������Ҹ� �ҷ����� �Լ�
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
