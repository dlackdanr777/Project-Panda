using System;
using UnityEngine;

[Serializable]
public class ServerStickerData
{
    public string Id;
    public Vector3 Pos;
    public Quaternion Rot;
    public Vector3 Scale;

    public ServerStickerData(string id, Vector3 pos, Quaternion rot, Vector3 scale)
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


    public ServerStickerData GetStickerData()
    {
        Quaternion rot = new Quaternion(RotX, RotY, RotZ, RotW);
        ServerStickerData returnData = new ServerStickerData(Id, Pos, rot, Scale);

        return returnData;
    }
}