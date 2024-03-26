using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public enum ETime
{
    Day,
    //Evening,
    Night
}

public class TimeManager : SingletonHandler<TimeManager>
{
    public DateTime TODAY => DateTime.Today;
    public int GameHour;
    public string GameHourId;
    public string GameWeatherId;

    public int DayEndTime = 17; // �� ���� �ð�
    //public int EveningEndTime = 21; // ���� ���� �ð�
    public int NightEndTime = 7; // �� ���� �ð�

    //private float _oneHour = 10 * 60; // ���ӿ����� �� �ð��� 10��
    //private float _checkHour = 0; // ���ӿ��� �� �ð��� �������� Ȯ��

    private Dictionary<string, MapData> _mapDic => _mapDatabase.GetMapDic();
    private ETime _eTime;
    public ETime ETime => _eTime;
    public string CurrentMap;
    private Light2D _light;

    private MapDatabase _mapDatabase;
    public MapDatabase MapDatabase => _mapDatabase;

    public override void Awake()
    {
        base.Awake();
        CurrentMap = "MN01";
        _mapDatabase = new MapDatabase();
        SceneManager.sceneLoaded += LoadedSceneEvent;
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= LoadedSceneEvent;
    }


    public void LoadedSceneEvent(Scene scene, LoadSceneMode mode)
    {
        FindLight();
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
                _eTime = ETime.Night;
                _light.intensity = 0.26f;
                break;
            case int hour when hour < DayEndTime: // ��
                _eTime = ETime.Day;
                _light.intensity = 1f;
                break;
            //case int hour when hour < EveningEndTime: // ����
            //    _eTime = ETime.Evening;
            //    _light.intensity = 0.5f;
            //    break;
            default: // ��
                _eTime = ETime.Night;
                _light.intensity = 0.26f;
                break;
        }

        //foreach (string key in _mapDic.Keys) // ���� �ִ� ���� ��游 ������ ���� + �ڽ� ������Ʈ�� ������ �����ϱ�
        //{
            if (_mapDic[CurrentMap].BackGround[(int)_eTime] != null)
            {
                // ��� �ð� ��� ����
                for (int i = 0; i < _mapDic[CurrentMap].BackGround.Length; i++)
                {
                    foreach (Transform child in _mapDic[CurrentMap].BackGround[i].transform)
                    {
                        child.gameObject.SetActive(false);
                    }
                    //_mapDic[key].BackGround[i]?.SetActive(false);
                }

                // �̹� �ð� ��� �ѱ�
                foreach (Transform child in _mapDic[CurrentMap].BackGround[(int)_eTime].transform)
                {
                    child.gameObject.SetActive(true);
                }
                //_mapDic[key].BackGround[(int)_eTime].SetActive(true);
            //}
        }
    }


    private void FindLight()
    {
        GameObject lightObj = GameObject.Find("GlobalLight2D");
        if (lightObj != null)
        {
            _light = lightObj.GetComponent<Light2D>();
        }
        else
        {
            _light = null;
            Debug.Log("����Ʈ�� �������� �ʴ� ���Դϴ�.");
        }
    }

}
