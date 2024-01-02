using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemImage", menuName = "Scriptable Object/ItemImage", order =int.MaxValue)]
public class ItemSpriteDatabase : ScriptableObject
{
    public ItemSprite[] ItemSprites;
}
