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

public class MapDatabase: SingletonHandler<MapDatabase>
{
    private Dictionary<string, MapData> _mapDic = new Dictionary<string, MapData>();
    private List<Dictionary<string, object>> _dataMap;

    public override void Awake()
    {
        base.Awake();
    }
    public void Start()
    {
        _dataMap = CSVReader.Read("Map");

        for (int i = 0; i < _dataMap.Count; i++)
        {
            string id = _dataMap[i]["ID"].ToString();
            string name = _dataMap[i]["ÀÌ¸§"].ToString();
            SpriteRenderer backGroundRenderer = null;
            if (id != "MN07" && id != "MN08")
            {
                backGroundRenderer = GameObject.Find(id + "BackGround").GetComponent<SpriteRenderer>();
            }


            MapData data = new MapData(id, name, backGroundRenderer);
            _mapDic.Add(id, data);
        }
    }

    public Dictionary<string, MapData> GetMapDic()
    {
        return _mapDic;
    }
}
