using Muks.WeightedRandom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserInfo
{

    public string UserId;    //유저 아이디

    public static DateTime TODAY => DateTime.Now;    //컴퓨터의 현재 날짜와 시간을 가져옴(추후 서버 시간으로 변경해야함)

    public DateTime _lastAccessDay; //마지막 접속일
    public int DayCount; //몇일 접속했나?

    public Dictionary<int, ItemData> DicRewardedItems;


    //유저의 데이터를 가져오는 함수
    public void LoadUserInfoData()
    {

    }




    [SerializeField] private List<string> _weekWeathers;
    private WeightedRandom<string> _weatherDatas;

}
