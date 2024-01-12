using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FurniturePositionDatabase
{
    private Dictionary<Vector3, string> _saveFurnitureList = new Dictionary<Vector3, string>();
    private string _fileName => "FurniturePosData.json";

    public void Register()
    {
        //Load();
    }
    public Dictionary<Vector3, string> GetSaveFurniutreList()
    {
        return _saveFurnitureList;
    }

    public void AddFurniturePosition(string id, Vector3 position)
    {
        // Dictionary�� id�� position �߰�
        _saveFurnitureList.Add(position, id);
    }

    public void RemoveFurniturePosition(Vector3 position)
    {
        if (_saveFurnitureList.ContainsKey(position))
        {
            _saveFurnitureList.Remove(position);
        }
    }

    public void Save()
    {
        if (_saveFurnitureList.Count == 0)
        {
            Debug.Log("���� �����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }
        string json = JsonUtility.ToJson(_saveFurnitureList, true);


        File.WriteAllText(UserInfo.PhotoPath + _fileName, json);
    }
    private void Load()
    {
        if (!File.Exists(UserInfo.PhotoPath + _fileName))
        {
            Debug.Log("���� ������ ���� ������ �������� �ʽ��ϴ�.");
            return;
        }

        string loadJson = File.ReadAllText(UserInfo.PhotoPath + _fileName);
        Debug.Log(loadJson);
        _saveFurnitureList = JsonUtility.FromJson<Dictionary<Vector3, string>>(loadJson);
    }

}
