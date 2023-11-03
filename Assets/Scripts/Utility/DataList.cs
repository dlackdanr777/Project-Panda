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
    /// 데이터를 리스트에 저장
    /// </summary>
    public void Add(T data)
    {
        _data.Add(data);
    }


    /// <summary>
    /// 데이터를 받아 리스트의 해당 데이터를 삭제
    /// </summary>
    public void Remove(T data)
    {
        if (_data.Contains(data))
        {
            _data.Remove(data);
        }
        else
        {
            Debug.LogError("해당 리스트에 지우고자 하는 데이터가 없습니다.");
        }
    }


    /// <summary>
    /// 인덱스를 받아 리스트의 데이터를 삭제
    /// </summary>
    public void RemoveAt(int index)
    {
        if (index >= _data.Count)
        {
            Debug.LogError("해당 인덱스가 List 저장 범위를 초과합니다.");
            return;
        }
        else if(index < 0)
        {
            Debug.LogError("해당 인덱스가 음수 입니다.");
            return;
        }
        
        _data.RemoveAt(index);
    }

    
    /// <summary>
    /// 인덱스를 받아 리스트의 해당 데이터를 반환
    /// </summary>
    public T GetData(int index)
    {
        if (index >= _data.Count)
        {
            Debug.LogError("해당 인덱스가 List 저장 범위를 초과합니다.");
            return default;
        }
        else if (index < 0)
        {
            Debug.LogError("해당 인덱스가 음수 입니다.");
            return default;
        }

        return _data[index];
    }


    /// <summary>
    /// 데이터가 저장된 리스트를 반환
    /// </summary>
    public List<T> GetList()
    {
        return _data;
    }

    /// <summary>
    /// 데이터 리스트를 복사해 복사본을 반환
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
    /// 리스트를 받아 데이터 리스트에 저장
    /// </summary>
    public void SetList(List<T> list)
    {
        _data = list;
    }

}
