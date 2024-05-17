using System;
using UnityEngine;

[Serializable]
public class Item
{
    protected string _id;
    public string Id => _id;

    protected string _name;
    public string Name => _name;

    protected string _description;
    public string Description => _description;

    protected string _rank;
    public string Rank => _rank;

    protected int _price;
    public int Price => _price;

    protected string _map;
    public string Map => _map;

    protected Sprite _image;
    public Sprite Image => _image;

    public bool IsReceived;

    public bool DiaryAlarmCheck;


    public Item(string id, string name, string description, int price, string rank, string map, Sprite image)
    {
        _id = id;
        _name = name;
        _description = description;
        _price = price;
        _rank = rank;
        _map = map;
        _image = image;
    }
}