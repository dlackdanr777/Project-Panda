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
        // Dictionary에 id와 position 추가
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
            Debug.Log("가구 데이터가 존재하지 않습니다.");
            return;
        }
        string json = JsonUtility.ToJson(_saveFurnitureList, true);


        File.WriteAllText(UserInfo.PhotoPath + _fileName, json);
    }
    private void Load()
    {
        if (!File.Exists(UserInfo.PhotoPath + _fileName))
        {
            Debug.Log("가구 데이터 저장 문서가 존재하지 않습니다.");
            return;
        }

        string loadJson = File.ReadAllText(UserInfo.PhotoPath + _fileName);
        Debug.Log(loadJson);
        _saveFurnitureList = JsonUtility.FromJson<Dictionary<Vector3, string>>(loadJson);
    }

}
