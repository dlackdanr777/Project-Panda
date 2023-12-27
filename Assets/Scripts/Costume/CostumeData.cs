using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EBodyParts
{
    None = -1,
    Head,
    LeftHand,
    RightHand
}

public class CostumeData
{
    public int CostumeID;
    public EBodyParts BodyParts;
    public string CostumeName;
    public Vector3 CostumePosition;
    public bool IsMine;
    public Sprite Image;
    public GameObject CostumeSlot;

    public CostumeData(int costumeID, EBodyParts bodyParts, string costumeName, float x, float y, float z)
    {
        CostumeID = costumeID;
        BodyParts = bodyParts;
        CostumeName = costumeName;
        CostumePosition = new Vector3(x, y, z);
    }
}
