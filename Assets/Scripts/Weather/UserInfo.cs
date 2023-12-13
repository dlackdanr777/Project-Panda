using Muks.WeightedRandom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserInfo
{

    //���� ������
    //==========================================================================================================

    public string UserId;    //���̵�
    public DateTime TODAY => DateTime.Now;    //��ǻ���� ���� ��¥�� �ð��� ������(���� ���� �ð����� �����ؾ���)

    public DateTime LastAccessDay => DateTime.Parse(_lastAccessDay); //������ ������

    public string _lastAccessDay;

    public int DayCount; //���� �����߳�?

    public bool IsTodayRewardReceipt; //���� �������� �����߳�?

    public bool IsExistingUser; //���� �����ΰ�?


    //==========================================================================================================

    //���� ������ ���� ��� (���� DB�� ���ε��ؾ���)
    private static string _path => Path.Combine(Application.dataPath, "UserInfo.json");

    //public static string PhotoPath => Application.persistentDataPath;

    public static string PhotoPath => "Data/";


    public void Register()
    {
        LoadUserInfoData();
    }


    //������ �����͸� �������� �Լ�
    public void LoadUserInfoData()
    {
        Debug.Log(_path);
        if (!File.Exists(_path))
        {
            Debug.Log("���� ���� ������ �������� �ʽ��ϴ�.");
            return;
        }

        UserInfo userInfo = new UserInfo();

        string loadJson = File.ReadAllText(_path);
        Debug.Log(loadJson);
        userInfo = JsonUtility.FromJson<UserInfo>(loadJson);

        UserId = userInfo.UserId;
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;
        DayCount = userInfo.DayCount;
        IsTodayRewardReceipt = userInfo.IsTodayRewardReceipt;
        IsExistingUser = userInfo.IsExistingUser;
    }

    public void SaveUserInfoData()
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(_path, json);
        Debug.Log(_path);
    }


}
