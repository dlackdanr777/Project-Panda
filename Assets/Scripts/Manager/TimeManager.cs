using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : SingletonHandler<TimeManager>
{
    public DateTime TODAY => DateTime.Now;
    public int GameHour;
    public string GameHourId;
    public string GameWeatherId;

    private float _oneHour = 10 * 60; // 게임에서의 한 시간은 10분
    private float _checkHour = 0; // 게임에서 한 시간이 지났는지 확인

    private Dictionary<string, MapData> _mapDic;
    private Sprite mapBackGround;
    public Sprite[] mapBackGrounds;

    public override void Awake()
    {
        var obj = FindObjectsOfType<TimeManager>();
        if (obj.Length == 1)
        {
            base.Awake();
        }
        else
        {
            Destroy(gameObject);
            TimeManager.Instance.CheckTime();
            return;
        }

        _mapDic = DatabaseManager.Instance.GetMapDic();
        CheckTime();
    }

    //void Update()
    //{
        //_checkHour += Time.deltaTime;
        //if(_checkHour > _oneHour)
        //{
        //    _checkHour = 0;
        //    CheckTime();
        //}
    //}

    public void CheckTime()
    {
        GameHour = ((TODAY.Hour % 4) * 60 + TODAY.Minute) / 10;

        if(GameHourId != "GTS" + (21 + GameHour)) // 게임 시간이 변경되었다면 계절 변경
        {
            switch (TODAY.Day)
            {
                case <= 7:
                    GameWeatherId = "WSP";
                    break;
                case <= 15:
                    GameWeatherId = "WSU";
                    break;
                case <= 22:
                    GameWeatherId = "WFA";
                    break;
                default:
                    GameWeatherId = "WWT";
                    break;
            }

            ChangedBackGround();
        }

        GameHourId = "GTS" + (21 + GameHour);
        Debug.Log("게임 시간 변경 GameHour: " + GameHour + "GameWeatherId: " + GameWeatherId);
    }

    /// <summary>
    /// 시간에 따라 배경 변경 </summary>
    private void ChangedBackGround()
    {
        // 맵 데이터베이스에 맵 배경 정보 추가
        // 시간 변경될 시 맵 배경 변경
        switch (GameHour)
        {
            case < 5: // 밤
                mapBackGround = mapBackGrounds[2];
                break;
            case < 18: // 낮
                mapBackGround = mapBackGrounds[0];
                break;
            case < 19: // 노을
                mapBackGround = mapBackGrounds[1];
                break;
            default: // 밤
                mapBackGround = mapBackGrounds[2];
                break;
        }

        foreach(string key in _mapDic.Keys)
        {
            if(key != "MN04" && key != "MN07" && key != "MN08")
            _mapDic[key].BackGroundRenderer.sprite = mapBackGround;
        }
    }
}
