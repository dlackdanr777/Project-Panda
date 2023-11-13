using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class WeatherData
{
    //이름
    [SerializeField] private string _name;
    public string Name => _name;

    //이미지
    [SerializeField] private Sprite _sprite;
    public Sprite Sprite => _sprite;

    //보상
    [SerializeField] private string _reward;
    public string Reward => _reward;

    //갯수
    [SerializeField] private int _amount;
    public int Amount => _amount;

}
