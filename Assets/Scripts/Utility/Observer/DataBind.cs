using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataBind : MonoBehaviour
{
    private static Dictionary<string, BindData<string>> _dataBindingText;

    private static Dictionary<string, BindData<UnityAction>> _dataBindingAction;

    private static Dictionary<string, BindData<Sprite>> _dataBindingSprite;

    /// <summary>
    /// �ؽ�Ʈ �������� ������ҿ� ���� ���� �����صδ� �Լ�(data���� ���� ��� GetTextValue�� ����� ���� ���� ����)
    /// </summary>
    public static void SetTextValue(string dataID, string data)
    {
        if (_dataBindingText == null)
            _dataBindingText = new Dictionary<string, BindData<string>>();

        if (!_dataBindingText.TryGetValue(dataID, out BindData<string> textData))
        {
            textData = new BindData<string>();
            _dataBindingText.Add(dataID, textData);
        }

        textData.Item = data;
    }


    /// <summary>
    /// ����� �ؽ�Ʈ �������� ������Ҹ� �ҷ��� �����ϴ� �Լ�
    /// </summary>
    public static BindData<string> GetTextValue(string dataID)
    {
        if (_dataBindingText == null)
            _dataBindingText = new Dictionary<string, BindData<string>>();

        if (!_dataBindingText.TryGetValue(dataID, out BindData<string> textData))
        {
            textData = new BindData<string>();
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
            _dataBindingAction = new Dictionary<string, BindData<UnityAction>>();

        if (!_dataBindingAction.TryGetValue(dataID, out BindData<UnityAction> buttonData))
        {
            buttonData = new BindData<UnityAction>();
            _dataBindingAction.Add(dataID, buttonData);
        }
        buttonData.Item = action;
    }


    /// <summary>
    /// ��ư �ؽ�Ʈ �������� ������Ҹ� �ҷ����� �Լ�
    /// </summary>
    public static BindData<UnityAction> GetButtonValue(string dataID)
    {
        if(_dataBindingAction == null)
            _dataBindingAction= new Dictionary<string, BindData<UnityAction>>();

        if(!_dataBindingAction.TryGetValue(dataID, out BindData<UnityAction> buttonData))
        {
            buttonData = new BindData<UnityAction>();
            _dataBindingAction.Add(dataID, buttonData);
        }
        return buttonData;
    }


    /// <summary>
    /// ��ư �������� ������ҿ� �������� �����صδ� �Լ�(action���� ���� ��� GetButtonValue�� ����� ���� ���� ����)
    /// </summary>
    public static void SetImageValue(string dataID, Sprite sprite)
    {
        if (_dataBindingSprite == null)
            _dataBindingSprite = new Dictionary<string, BindData<Sprite>>();

        if (!_dataBindingSprite.TryGetValue(dataID, out BindData<Sprite> imageData))
        {
            imageData = new BindData<Sprite>();
            _dataBindingSprite.Add(dataID, imageData);
        }
        imageData.Item = sprite;
    }


    /// <summary>
    /// ��ư �ؽ�Ʈ �������� ������Ҹ� �ҷ����� �Լ�
    /// </summary>
    public static BindData<Sprite> GetImageValue(string dataID)
    {
        if (_dataBindingSprite == null)
            _dataBindingSprite = new Dictionary<string, BindData<Sprite>>();

        if (!_dataBindingSprite.TryGetValue(dataID, out BindData<Sprite> imageData))
        {
            imageData = new BindData<Sprite>();
            _dataBindingSprite.Add(dataID, imageData);
        }
        return imageData;
    }

}
