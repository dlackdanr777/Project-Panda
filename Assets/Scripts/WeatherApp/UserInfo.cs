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

        //���� �ð��� ����
        RewardedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:s");

        //����ǰ�� �������� �������� ������?
        if (!DicRewardedItems.ContainsKey(rewarededItem.Id))
        {
            //��ųʸ��� �������� �߰��Ѵ�.
            DicRewardedItems.Add(rewarededItem.Id, rewarededItem);
        }
        //���� �ϸ�?
        else
        {
            //������ �ø���.
            DicRewardedItems[rewarededItem.Id].Amount += rewarededItem.Amount;
            Debug.LogFormat("{0}(��������: {1} + {2}ȹ��", DicRewardedItems[rewarededItem.Id].Name, DicRewardedItems[rewarededItem.Id].Amount, rewarededItem.Amount);   
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
        //����� ���丮 ��Ÿ ������ ���
        DirectoryInfo folder = new DirectoryInfo("./info");
        
        //�ش� ���丮�� �ִ��� Ȯ��
        if (!folder.Exists)
        {
            //������� �ش� ����� ���丮 ����
            folder.Create();
        }

        string path = "./info/userInfo.json";

        try
        {
            //�ش��ο� ������ �������
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                UserInfo readInfo = new UserInfo();

                //�ش� ����� ������ ������ȭ�Ͽ� UserInfoŬ������ ��ȭ��Ű�� �����Ѵ�.
                readInfo = JsonConvert.DeserializeObject<UserInfo>(json);

                //���������͸� �޾ƿ� �����Ѵ�.
                RewardedDate = readInfo.RewardedDate;
                UserId = readInfo.UserId;
                DicRewardedItems = readInfo.DicRewardedItems;
                DayCount = readInfo.DayCount;
                RewardedCount = readInfo.RewardedCount;
            }

            //���� ���
            else
            {
                //���ο� ���� ���̵� ����
                UserId = "lion";
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }

        DayCount++;

        //���� ������ ������ �ƴ϶��?
        if (!RewardedCountCheck())
        {
            RewardedCount = 0;
        }
        RewardedCount++;
    }

    public bool RewardedCountCheck()
    {
        //���ڿ��ε� ��¥�� DateTime���� ��ȯ�Ͽ� ����
        DateTime loadDate = Convert.ToDateTime(RewardedDate);

        //���� ����� ��¥�� null�� �ƴ϶��
        if(RewardedDate != null)
        {
            //���� ���� ��¥ �Ϸ����� ������ ���� ��¥�� ������?
            if(WeatherApp.TODAY.AddDays(-1).Day == loadDate.Day)
            {
                return true;
            }
        }
        //������� ��¥�� ���糯¥�� ��ȯ�Ѵ�.
        RewardedDate = WeatherApp.TODAY.ToString("yyyy-MM-dd HH:mm:s");
        return false;
    }
}
