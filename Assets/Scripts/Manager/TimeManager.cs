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

    private float _oneHour = 10 * 60; // ���ӿ����� �� �ð��� 10��
    private float _checkHour = 0; // ���ӿ��� �� �ð��� �������� Ȯ��
    
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

        if(GameHourId != "GTS" + (21 + GameHour)) // ���� �ð��� ����Ǿ��ٸ� ���� ����
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
        Debug.Log("���� �ð� ���� GameHour: " + GameHour + "GameWeatherId: " + GameWeatherId);
    }
}
