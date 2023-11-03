using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UIView : MonoBehaviour
{
    public enum VisibleState
    {
        Disappeared, // �����
        Disappearing, //������� ��
        Appeared, //��Ÿ��
        Appearing, //��Ÿ������
    }

    public VisibleState _visibleState;

    static Dictionary<string, TextData> _dataBindingTextToDic = null;

    /// <summary>
    /// �ؽ�Ʈ �����͸� �����صδ� �Լ�
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
    /// ����� �����͸� �ҷ����� �Լ�
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
    /// UI�� �ҷ����� �ݹ�Ǵ� �Լ�
    /// </summary>
    public abstract void Show();


    /// <summary>
    /// UI�� ���� �ݹ�Ǵ� �Լ�
    /// </summary>
    public abstract void Hide();

}
