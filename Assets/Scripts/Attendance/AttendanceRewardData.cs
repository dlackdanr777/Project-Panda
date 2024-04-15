using System;
using UnityEngine;


public class AttendanceRewardData
{
    private int _day;
    public int Day => _day;

    private string _weather;
    public string Weather => _weather;

    //°¹¼ö
    private int _amount;
    public int Amount => _amount;

    private Item _item;
    public Item Item => _item;

    private Sprite _sprite;
    public Sprite Sprite => _sprite;

    public AttendanceRewardData(int day, string weather, int amount, Item item, Sprite sprite)
    {
        _day = day;
        _weather = weather;
        _item = item;
        _amount = amount;
        _sprite = sprite;
    }

}
