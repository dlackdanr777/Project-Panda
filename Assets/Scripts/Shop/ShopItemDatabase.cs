using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop Item Database", menuName = "Shop Item Database")]
public class ShopItemDatabase : ScriptableObject
{
    [SerializeField]
    private string _id;
    public string Id { get { return _id; } }

    [SerializeField]
    private string _name;
    public string Name { get { return _name; } }

    [SerializeField]
    private string _description;
    public string Description { get { return _description; } }

    [SerializeField]
    private int _price;
    public int Price { get { return _price; } }

    [SerializeField]
    private int _amount;
    public int Amount { get { return _amount; } set { _amount = value; } }

    [SerializeField]
    private Sprite _icon;
    public Sprite Icon { get { return _icon; } }
}
