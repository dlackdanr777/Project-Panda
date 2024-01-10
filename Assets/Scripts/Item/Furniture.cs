using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Furniture : Item
{
    public Vector2 FunrnitureCoor;
    public Vector2 PlayerCoor;
    public FurnitureType FurnitureType;

    public Furniture(string id, string name, string description, int price, string rank, string map, Sprite image) : base(id, name, description, price, rank, map, image) {}
}
