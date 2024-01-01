using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingSystem : MonoBehaviour
{
    [SerializeField] private UICooking _uiCooking;

    private RecipeDatabase _recipeDatabase; //=> DatabaseManager.Instance.RecipeDatabase;

    private Inventory[] _inventory => GameManager.Instance.Player.CookItemInventory;

    private RecipeData[] _recipeDatas;
    private void Start()
    {
        _recipeDatas = _recipeDatabase.GetRecipeDataArray();
    }

    private void Init()
    {

    }

    public bool CheckRecipe(InventoryItem item)
    {
        foreach(RecipeData data in _recipeDatas)
        {
            if (data.MaterialItemID == item.Id && data.MaterialValue <= item.Count)
            {
                return true;
            }
                
        }

        return false;
    }

    public void StartCooking(InventoryItem item)
    {
        Debug.Log("½ÃÀÛ");
        foreach (RecipeData data in _recipeDatas)
        {
            if (data.MaterialItemID == item.Id)
            {
                Debug.Log(data.MaterialValue);
                for(int i = 0, count = data.MaterialValue; i < count; i++)
                {
                    _inventory[0].Remove(item);
                }
                
                _uiCooking.UpdateUI();
                return;
            }
        }
    }
}
