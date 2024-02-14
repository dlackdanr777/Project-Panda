using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ETime
{
    Day,
    Evening,
    Night
}

public class TimeManager : SingletonHandler<TimeManager>
{
    public DateTime TODAY => DatabaseManager.Instance.UserInfo.TODAY;
    public int GameHour;
    public string GameHourId;
    public string GameWeatherId;

    private float _oneHour = 10 * 60; // ���ӿ����� �� �ð��� 10��
    private float _checkHour = 0; // ���ӿ��� �� �ð��� �������� Ȯ��

    private Dictionary<string, MapData> _mapDic => _mapDatabase.GetMapDic();
    private ETime eTime;
    private Sprite mapBackGround;
    public Sprite[] mapBackGrounds;

    private MapDatabase _mapDatabase;

    //void Update()
    //{
    //_checkHour += Time.deltaTime;
    //if(_checkHour > _oneHour)
    //{
    //    _checkHour = 0;
    //    CheckTime();
    //}
    //}


    public override void Awake()
    {
        base.Awake();
        _mapDatabase = new MapDatabase();
        SceneManager.sceneLoaded += LoadedSceneEvent;
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= LoadedSceneEvent;
    }


    public void LoadedSceneEvent(Scene scene, LoadSceneMode mode)
    {
        CheckMap();
    }


    /// <summary>�ð��� ���� Ȯ���� �����ϴ� �Լ�</summary> 
    public void CheckMap()
    {
        _mapDatabase.CheckMap();
        CheckTime();
        if (_mapDatabase.IsMapExists)
        {
            ChangedBackGround();
            Debug.Log("���� �����մϴ�.");
        }

        else
        {
            Debug.LogError("���� �������� �ʴ� �� �Դϴ�.");
        }
    }


    public void CheckTime()
    {
        DateTime now = TODAY;
        GameHour = ((now.Hour % 4) * 60 + now.Minute) / 10;

        if (GameHourId != "GTS" + (21 + GameHour)) // ���� �ð��� ����Ǿ��ٸ� ���� ����
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

    /// <summary>
    /// �ð��� ���� ��� ���� </summary>
    private void ChangedBackGround()
    {
        // �� �����ͺ��̽��� �� ��� ���� �߰�
        // �ð� ����� �� �� ��� ����
        switch (GameHour)
        {
            case < 5: // ��
                mapBackGround = mapBackGrounds[2];
                eTime = ETime.Night;
                break;
            case < 17: // ��
                mapBackGround = mapBackGrounds[0];
                eTime = ETime.Day;
                break;
            case < 21: // ����
                mapBackGround = mapBackGrounds[1];
                eTime = ETime.Evening;
                break;
            default: // ��
                mapBackGround = mapBackGrounds[2];
                eTime = ETime.Night;
                break;
        }

        foreach(string key in _mapDic.Keys)
        {
            if(key != "MN07" && key != "MN08") // ���߿� ����
            {
                _mapDic[key].BackGroundRenderer.sprite = mapBackGround;

                if (_mapDic[key].BackGround[(int)eTime] != null)
                {
                    // �̹� �ð� ��� �ѱ�
                    _mapDic[key].BackGround[(int)eTime].SetActive(true);

                    // �� �ð� ��� ����
                    if ((int)eTime == 0)
                    {
                        _mapDic[key].BackGround[System.Enum.GetValues(typeof(ETime)).Length].SetActive(false);
                    }
                    else
                    {
                        _mapDic[key].BackGround[(int)(eTime - 1)].SetActive(false);
                    }
                }  
            }

        }
    }
}
