using Muks.WeightedRandom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserInfo
{

    public static string UserId;    //���� ���̵�

    public static DateTime TODAY => DateTime.Now;    //��ǻ���� ���� ��¥�� �ð��� ������(���� ���� �ð����� �����ؾ���)

    public static DateTime _lastAccessDay; //������ ������

    public static int DayCount; //���� �����߳�?

    public static bool IsTodayRewardReceipt; //���� �������� �����߳�?

    public static bool IsExistingUser => false; //���� �����ΰ�?

    public static Dictionary<int, ItemData> DicRewardedItems;

    [SerializeField] private List<string> _weekWeathers;
    private WeightedRandom<string> _weatherDatas;



    //������ �����͸� �������� �Լ�
    public void LoadUserInfoData()
    {

    }


}
