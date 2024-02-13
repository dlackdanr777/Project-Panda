using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public string Id;
    public string Name;
    public SpriteRenderer BackGroundRenderer;

    public MapData(string id, string name, SpriteRenderer backGroundRenderer)
    {
        Id = id;
        Name = name;
        BackGroundRenderer = backGroundRenderer;
    }
}

public class MapDatabase
{
    private Dictionary<string, MapData> _mapDic = new Dictionary<string, MapData>();
    private static List<Dictionary<string, object>> _dataMap = new List<Dictionary<string, object>>();

    private bool _isMapExists;
    public bool IsMapExists => _isMapExists;


    public MapDatabase()
    {
        _dataMap = CSVReader.Read("Map");
    }


    public void CheckMap()
    {
        _mapDic.Clear();

        if (_dataMap.Count == 0)
        {
            _isMapExists = false;
            Debug.LogError("맵 데이터가 존재하지 않습니다.");
            return;
        }

        for (int i = 0; i < _dataMap.Count; i++)
        {
            string id = _dataMap[i]["ID"].ToString();
            string name = _dataMap[i]["이름"].ToString();
            SpriteRenderer backGroundRenderer = null;
            if (id != "MN07" && id != "MN08")
            {
                if (GameObject.Find(id + "BackGround") == null)
                {

                    _isMapExists = false;
                    return;
                }
                Debug.Log(id);
                backGroundRenderer = GameObject.Find(id + "BackGround").GetComponent<SpriteRenderer>();
            }


            MapData data = new MapData(id, name, backGroundRenderer);
            _mapDic.Add(id, data);
        }

        _isMapExists = true;
    }


    public Dictionary<string, MapData> GetMapDic()
    {
        return _mapDic;
    }
}
