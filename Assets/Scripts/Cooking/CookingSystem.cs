using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingSystem : MonoBehaviour
{
    private ItemDatabase _itemDatabase => DatabaseManager.Instance.ItemDatabase;

    private Inventory[] _inventory => GameManager.Instance.Player.Inventory;


    private void Init()
    {

    }
}
