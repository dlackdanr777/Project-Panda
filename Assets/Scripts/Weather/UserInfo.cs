using Muks.WeightedRandom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserInfo
{

    public string UserId;    //���� ���̵�

    public static DateTime TODAY => DateTime.Now;    //��ǻ���� ���� ��¥�� �ð��� ������(���� ���� �ð����� �����ؾ���)

    public DateTime _lastAccessDay; //������ ������
    public int DayCount; //���� �����߳�?

    public Dictionary<int, ItemData> DicRewardedItems;


    //������ �����͸� �������� �Լ�
    public void LoadUserInfoData()
    {

    }




    [SerializeField] private List<string> _weekWeathers;
    private WeightedRandom<string> _weatherDatas;

}
