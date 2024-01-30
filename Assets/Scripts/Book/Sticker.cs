using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Recorder.OutputPath;

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

    public SaveStickerData GetSaveStickerData()
    {
        float rotX = Rot.x;
        float rotY = Rot.y;
        float rotZ = Rot.z;
        float rotW = Rot.w;

        SaveStickerData returnData = new SaveStickerData(Id, Pos, rotX, rotY, rotZ, rotW, Scale);
        return returnData;
    }
}


public class SaveStickerData
{
    public string Id;
    public float RotX;
    public float RotY;
    public float RotZ;
    public float RotW;
    public Vector3 Pos;
    public Vector3 Scale;


    public SaveStickerData(string id, Vector3 pos, float rotX, float rotY, float rotZ, float rotW, Vector3 scale)
    {
        Id = id;
        Pos = pos;
        RotX = rotX;
        RotY = rotY;
        RotZ = rotZ;
        RotW = rotW;
        Scale = scale;
    }


    public StickerData GetStickerData()
    {
        Quaternion rot = new Quaternion(RotX, RotY, RotZ, RotW);
        StickerData returnData = new StickerData(Id, Pos, rot, Scale);

        return returnData;
    }
}
