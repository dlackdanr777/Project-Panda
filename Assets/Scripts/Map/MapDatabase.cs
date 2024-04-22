using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public string Id;
    public string Name;
    public GameObject[] BackGround;

    public MapData(string id, string name, GameObject[] backGround)
    {
        Id = id;
        Name = name;
        BackGround = backGround;
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

    public void Register()
    {
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
        string id = string.Empty;

        id = _dataMap[0]["ID"].ToString();
        if (GameObject.Find(id + "BackGround") == null)
        {
            _isMapExists = false;
            return;
        }

        for (int i = 0; i < _dataMap.Count; i++)
        {
            id = _dataMap[i]["ID"].ToString();
            string name = _dataMap[i]["Name"].ToString();
            GameObject[] backGround = new GameObject[System.Enum.GetValues(typeof(ETime)).Length];

            if (GameObject.Find(id + "BackGround") == null)
            {
                continue;
            }

            // 나중에 for문으로 수정
            backGround[(int)ETime.Day] = GameObject.Find(id + "DayBackGround")?.GetComponent<Transform>().gameObject;
            //backGround[(int)ETime.Evening] = GameObject.Find(id + "EveningBackGround")?.GetComponent<Transform>().gameObject;
            backGround[(int)ETime.Night] = GameObject.Find(id + "NightBackGround")?.GetComponent<Transform>().gameObject;

            for(int j = 0; j <backGround.Length; j++)
            {
                if (backGround[j] != null)
                {
                    foreach(Transform child in backGround[j].transform)
                    {
                        child.gameObject.SetActive(false);
                    }
                    //backGround[j].SetActive(false);
                }
            }


            MapData data = new MapData(id, name, backGround);
            _mapDic.Add(id, data);
        }

        _isMapExists = true;
    }


    public Dictionary<string, MapData> GetMapDic()
    {
        return _mapDic;
    }
}
