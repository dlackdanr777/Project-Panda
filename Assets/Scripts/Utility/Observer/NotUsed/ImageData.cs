using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ImageData
{
    public event Action<Sprite> CallBack;

    private Sprite _sprite;

    public Sprite Sprite
    {
        get { return _sprite; }
        set
        {
            _sprite = value;
            CallBack?.Invoke(_sprite);
        }
    }
}
