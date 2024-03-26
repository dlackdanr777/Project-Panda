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

    public int DayEndTime = 17; // 낮 종료 시간
    //public int EveningEndTime = 21; // 저녁 종료 시간
    public int NightEndTime = 7; // 밤 종료 시간

    //private float _oneHour = 10 * 60; // 게임에서의 한 시간은 10분
    //private float _checkHour = 0; // 게임에서 한 시간이 지났는지 확인

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


    /// <summary>시간과 맵을 확인해 변경하는 함수</summary> 
    public void CheckMap()
    {
        _mapDatabase.CheckMap();
        CheckTime();
        if (_mapDatabase.IsMapExists)
        {
            ChangedBackGround();
            Debug.Log("맵이 존재합니다.");
        }

        else
        {
            Debug.LogError("맵이 존재하지 않는 씬 입니다.");
        }
    }


    public void CheckTime()
    {
        DateTime now = TODAY;
        GameHour = ((now.Hour % 4) * 60 + now.Minute) / 10;

        if (GameHourId != "GTS" + (21 + GameHour)) // 게임 시간이 변경되었다면 계절 변경
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

    /// <summary>
    /// 시간에 따라 배경 변경 </summary>
    private void ChangedBackGround()
    {
        // 맵 데이터베이스에 맵 배경 정보 추가
        // 시간 변경될 시 맵 배경 변경
        switch (GameHour)
        {
            case int hour when hour < NightEndTime: // 밤
                _eTime = ETime.Night;
                _light.intensity = 0.26f;
                break;
            case int hour when hour < DayEndTime: // 낮
                _eTime = ETime.Day;
                _light.intensity = 1f;
                break;
            //case int hour when hour < EveningEndTime: // 저녁
            //    _eTime = ETime.Evening;
            //    _light.intensity = 0.5f;
            //    break;
            default: // 밤
                _eTime = ETime.Night;
                _light.intensity = 0.26f;
                break;
        }

        //foreach (string key in _mapDic.Keys) // 현재 있는 맵의 배경만 끄도록 수정 + 자식 오브젝트만 끄도록 수정하기
        //{
            if (_mapDic[CurrentMap].BackGround[(int)_eTime] != null)
            {
                // 모든 시간 배경 끄기
                for (int i = 0; i < _mapDic[CurrentMap].BackGround.Length; i++)
                {
                    foreach (Transform child in _mapDic[CurrentMap].BackGround[i].transform)
                    {
                        child.gameObject.SetActive(false);
                    }
                    //_mapDic[key].BackGround[i]?.SetActive(false);
                }

                // 이번 시간 배경 켜기
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
            Debug.Log("라이트가 존재하지 않는 맵입니다.");
        }
    }

}
