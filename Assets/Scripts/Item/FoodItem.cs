using System;
using UnityEngine;

public class FoodItem : Item
{
    private string _mbti;
    public string Mbti => _mbti;

    public FoodItem(string id, string name, string description, int price, string rank, string mbti, Sprite image) : base(id, name, description, price, rank, null, image)
    {
        _mbti = mbti;
    }
}
