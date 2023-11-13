using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class WeatherData
{
    //�̸�
    [SerializeField] private string _name;
    public string Name => _name;

    //�̹���
    [SerializeField] private Sprite _sprite;
    public Sprite Sprite => _sprite;

    //����
    [SerializeField] private string _reward;
    public string Reward => _reward;

    //����
    [SerializeField] private int _amount;
    public int Amount => _amount;

}
