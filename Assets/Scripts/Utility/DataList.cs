using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataList<T>
{
    [SerializeField] private List<T> _data = new List<T>();
    
    public int Count => _data.Count;


    /// <summary>
    /// �����͸� ����Ʈ�� ����
    /// </summary>
    public void Add(T data)
    {
        _data.Add(data);
    }


    /// <summary>
    /// �����͸� �޾� ����Ʈ�� �ش� �����͸� ����
    /// </summary>
    public void Remove(T data)
    {
        if (_data.Contains(data))
        {
            _data.Remove(data);
        }
        else
        {
            Debug.LogError("�ش� ����Ʈ�� ������� �ϴ� �����Ͱ� �����ϴ�.");
        }
    }


    /// <summary>
    /// �ε����� �޾� ����Ʈ�� �����͸� ����
    /// </summary>
    public void RemoveAt(int index)
    {
        if (index >= _data.Count)
        {
            Debug.LogError("�ش� �ε����� List ���� ������ �ʰ��մϴ�.");
            return;
        }
        else if(index < 0)
        {
            Debug.LogError("�ش� �ε����� ���� �Դϴ�.");
            return;
        }
        
        _data.RemoveAt(index);
    }

    
    /// <summary>
    /// �ε����� �޾� ����Ʈ�� �ش� �����͸� ��ȯ
    /// </summary>
    public T GetData(int index)
    {
        if (index >= _data.Count)
        {
            Debug.LogError("�ش� �ε����� List ���� ������ �ʰ��մϴ�.");
            return default;
        }
        else if (index < 0)
        {
            Debug.LogError("�ش� �ε����� ���� �Դϴ�.");
            return default;
        }

        return _data[index];
    }


    /// <summary>
    /// �����Ͱ� ����� ����Ʈ�� ��ȯ
    /// </summary>
    public List<T> GetList()
    {
        return _data;
    }

    /// <summary>
    /// ������ ����Ʈ�� ������ ���纻�� ��ȯ
    /// </summary>
    public List<T> Clone()
    {
        List<T> list = new List<T>();
        foreach (T data in _data)
        {
            list.Add(data);
        }

        return list;
    }


    /// <summary>
    /// ����Ʈ�� �޾� ������ ����Ʈ�� ����
    /// </summary>
    public void SetList(List<T> list)
    {
        _data = list;
    }

}
