using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CostumeImage", menuName = "Scriptable Object/CostumeImage", order = int.MaxValue)]
public class CostumeImage : ScriptableObject
{
    public Sprite[] HeadCostumeImages;
    public Sprite[] LeftHandCostumeImages;
    public Sprite[] RightCostumeImages;
}
