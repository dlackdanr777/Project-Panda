using Muks.DataBind;
using UnityEngine;
using UnityEngine.UI;

public class UIItemList : UIList<Item>
{
    private int _currentItemIndex;
    private Database_Ssun _dataBase;

    public Sprite Test;

    private void Awake()
    {
        _dataBase = Database_Ssun.Instance;
        GameManager.Instance.Player.Inventory[1].AddById(Field.Snack, "I03"); //player가 아이템 하나를 얻음
        
        //Test snack
        for(int i = 0; i < 2; i++)
        {
            _maxCount[i] = _dataBase.ItemCount[i];
            _lists[i] = _dataBase.ItemList[i];
        }

        Init();
    }

    protected override void GetContent(int index)
    {
        DataBind.SetTextValue("ItemDetailName", _lists[(int)_currentField][index].Name);
        DataBind.SetTextValue("ItemDetailDescription", _lists[(int)_currentField][index].Description);
        DataBind.SetSpriteValue("ItemDetailImage", _lists[(int)_currentField][index].Image);
    }

    protected override void UpdateListSlots()
    {
        for (int j = 0; j < _maxCount[(int)_currentField]; j++)
        {
            if (_lists[(int)_currentField][j].IsReceived) //인벤토리에 있는 아이템이라면
            {
                _spawnPoint[(int)_currentField].GetChild(j).GetComponent<Button>().interactable = true;
                _spawnPoint[(int)_currentField].GetChild(j).GetComponent<Image>().sprite = _lists[(int)_currentField][j].Image;
            }
        }
    }
}
