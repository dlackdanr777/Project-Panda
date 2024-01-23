using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticker
{
    public string Id;
    public Sprite Image;

    public Sticker(string id, Sprite image)
    {
        Id = id;
        Image = image;
    }   
}

[Serializable]
public class StickerData
{
    public string Id;
    public Vector3 Pos;
    public Quaternion Rot;
    public Vector3 Scale;

    public StickerData(string id, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        Id = id;
        Pos = pos;
        Rot = rot;
        Scale = scale;
    }
}
