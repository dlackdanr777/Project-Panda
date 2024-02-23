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

    public int DayEndTime = 17; // �� ���� �ð�
    public int EveningEndTime = 21; // ���� ���� �ð�
    public int NightEndTime = 7; // �� ���� �ð�

    //private float _oneHour = 10 * 60; // ���ӿ����� �� �ð��� 10��
    //private float _checkHour = 0; // ���ӿ��� �� �ð��� �������� Ȯ��

    private Dictionary<string, MapData> _mapDic => _mapDatabase.GetMapDic();
    private ETime eTime;
    private Sprite mapBackGround;
    public Sprite[] mapBackGrounds;

    private MapDatabase _mapDatabase;

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
            case int hour when hour < NightEndTime: // ��
                mapBackGround = mapBackGrounds[2];
                eTime = ETime.Night;
                break;
            case int hour when hour < DayEndTime: // ��
                mapBackGround = mapBackGrounds[0];
                eTime = ETime.Day;
                break;
            case int hour when hour < EveningEndTime: // ����
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
            //_mapDic[key].BackGroundRenderer.sprite = mapBackGround;
            if (_mapDic[key].BackGround[(int)eTime] != null)
            {
                // ��� �ð� ��� ����
                for(int i = 0; i < _mapDic[key].BackGround.Length; i++)
                {
                    _mapDic[key].BackGround[i]?.SetActive(false);
                }

                // �̹� �ð� ��� �ѱ�
                _mapDic[key].BackGround[(int)eTime].SetActive(true);
            }
        }
    }
}
