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
    public static DateTime TODAY => DateTime.Now;    //��ǻ���� ���� ��¥�� �ð��� ������(���� ���� �ð����� �����ؾ���)

    public static DateTime _lastAccessDay; //������ ������

    public static int DayCount; //���� �����߳�?

    public static bool IsTodayRewardReceipt; //���� �������� �����߳�?

    public static bool IsExistingUser => false; //���� �����ΰ�?

    public static Dictionary<int, ItemData> DicRewardedItems;

    private static List<string> _weekWeathers;

    private static WeightedRandom<string> _weatherDatas;

    //==========================================================================================================

    //���� ������ ���� ��� (���� DB�� ���ε��ؾ���)
    private static string _path => Path.Combine(Application.dataPath, "UserInfo.json");

    public static string PhotoPath => Application.persistentDataPath;

    //������ �����͸� �������� �Լ�
    public void LoadUserInfoData()
    {

    }

    public static void SaveUserInfoData()
    {
        UserInfo userInfo = new UserInfo();
        string json = JsonUtility.ToJson(userInfo, true);
        File.WriteAllText(_path, json);
        Debug.Log(_path);
    }


}
