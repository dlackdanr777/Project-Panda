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

    public static Dictionary<int, ItemData> DicRewardedItems;


    //������ �����͸� �������� �Լ�
    public void LoadUserInfoData()
    {

    }
    [SerializeField] private List<string> _weekWeathers;
    private WeightedRandom<string> _weatherDatas;

}
