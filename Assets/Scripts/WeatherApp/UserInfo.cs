using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserInfo
{
    public string UserId;
    public string RewardedDate;

    public int DayCount;
    public int RewardedCount;

    public Dictionary<int, ItemData> DicRewardedItems;

    public UserInfo() { }

    public UserInfo(string userId, string rewardedDate, Dictionary<int, ItemData> dicRewardedItems)
    {
        UserId = userId;
        RewardedDate = rewardedDate;
        DicRewardedItems = dicRewardedItems;
    }

    public bool RewarededItem(string userId, ItemData rewarededItem)
    {
        UserId = userId;

        //현재 시간을 저장
        RewardedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:s");

        //소지품에 아이템이 존재하지 않으면?
        if (!DicRewardedItems.ContainsKey(rewarededItem.Id))
        {
            //딕셔너리에 아이템을 추가한다.
            DicRewardedItems.Add(rewarededItem.Id, rewarededItem);
        }
        //존재 하면?
        else
        {
            //갯수를 늘린다.
            DicRewardedItems[rewarededItem.Id].Amount += rewarededItem.Amount;
            Debug.LogFormat("{0}(소지수량: {1} + {2}획득", DicRewardedItems[rewarededItem.Id].Name, DicRewardedItems[rewarededItem.Id].Amount, rewarededItem.Amount);   
        }
        return SaveUserInfoData();
    }

    public bool SaveUserInfoData()
    {
        bool result = false;
        var json = JsonConvert.SerializeObject(this);

        try
        {
            File.WriteAllText("./info/userInfo.json", json);

            result = true;
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }

        return result;
    }

    public void LoadUserInfoData()
    {
        //경로의 디렉토리 메타 데이터 취득
        DirectoryInfo folder = new DirectoryInfo("./info");
        
        //해당 디렉토리가 있는지 확인
        if (!folder.Exists)
        {
            //없을경우 해당 경로의 디렉토리 생성
            folder.Create();
        }

        string path = "./info/userInfo.json";

        try
        {
            //해당경로에 파일이 있을경우
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                UserInfo readInfo = new UserInfo();

                //해당 경로의 파일을 역직렬화하여 UserInfo클래스로 변화시키고 저장한다.
                readInfo = JsonConvert.DeserializeObject<UserInfo>(json);

                //유저데이터를 받아와 저장한다.
                RewardedDate = readInfo.RewardedDate;
                UserId = readInfo.UserId;
                DicRewardedItems = readInfo.DicRewardedItems;
                DayCount = readInfo.DayCount;
                RewardedCount = readInfo.RewardedCount;
            }

            //없을 경우
            else
            {
                //새로운 유저 아이디 생성
                UserId = "lion";
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }

        DayCount++;

        //만약 보상은 받은게 아니라면?
        if (!RewardedCountCheck())
        {
            RewardedCount = 0;
        }
        RewardedCount++;
    }

    public bool RewardedCountCheck()
    {
        //문자열로된 날짜를 DateTime으로 변환하여 저장
        DateTime loadDate = Convert.ToDateTime(RewardedDate);

        //만약 저장된 날짜가 null이 아니라면
        if(RewardedDate != null)
        {
            //만약 현재 날짜 하루전과 보상을 받은 날짜가 같으면?
            if(WeatherApp.TODAY.AddDays(-1).Day == loadDate.Day)
            {
                return true;
            }
        }
        //보상받은 날짜를 현재날짜로 변환한다.
        RewardedDate = WeatherApp.TODAY.ToString("yyyy-MM-dd HH:mm:s");
        return false;
    }
}
