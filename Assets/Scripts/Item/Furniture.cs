using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Furniture : Item
{
    public Vector2 FunrnitureCoor;
    public Vector2 PlayerCoor;
    public FurnitureType FurnitureType;

    public Furniture(string id, string name, string description, int price, Sprite image) : base(id, name, description, price, image) {}
}
