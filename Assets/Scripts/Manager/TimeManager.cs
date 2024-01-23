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
        }

        GameHourId = "GTS" + (21 + GameHour);
        Debug.Log("게임 시간 변경 GameHour: " + GameHour + "GameWeatherId: " + GameWeatherId);
    }
}
