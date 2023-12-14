using Muks.WeightedRandom;
using Newtonsoft.Json;
using System;
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

            CreateUserInfoData();
            return;
        }

        UserInfo userInfo = new UserInfo();

        string loadJson = File.ReadAllText(_path);
        userInfo = JsonUtility.FromJson<UserInfo>(loadJson);

        UserId = userInfo.UserId;
        string paser = userInfo._lastAccessDay.ToString();
        _lastAccessDay = paser;
        DayCount = userInfo.DayCount;  
        IsExistingUser = userInfo.IsExistingUser;

        IsTodayRewardReceipt = true;

        if (TODAY.Day > LastAccessDay.Day)
        {
            Debug.Log("����");
            DayCount++;
            IsTodayRewardReceipt = false;
        }
    }

    private void CreateUserInfoData()
    {
        IsExistingUser = false;
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

        DayCount++;
        IsTodayRewardReceipt = false;

        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(_path, json);
    }


    public void SaveUserInfoData()
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(_path, json);
        Debug.Log(_path);
    }


}
