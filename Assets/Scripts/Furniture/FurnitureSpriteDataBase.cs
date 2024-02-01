using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FurnitureImage", menuName = "Scriptable Object/FurnitureImage", order = int.MaxValue)]
public class FurnitureSpriteDatabase : ScriptableObject
{
    public FurnitureSprite[] ItemSprites;
}

[Serializable]
public class FurnitureSprite
{
    public string Id;
    public Sprite Image;
    public Sprite Thumbnails;
}
