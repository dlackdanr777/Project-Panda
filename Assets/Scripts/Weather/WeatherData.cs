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

    //날씨 이미지
    [SerializeField] private Sprite _weatherSprite;
    public Sprite WeatherSprite => _weatherSprite;

    //보상 이미지
    [SerializeField] private Sprite _rewardSprite;
    public Sprite RewardSprite => _rewardSprite;

    //보상
    //나중에 배열로 변경하여 보상 목록을 보여줘야함
    [SerializeField] private string _reward;
    public string Reward => _reward;

    //갯수
    [SerializeField] private int _amount;
    public int Amount => _amount;

}
