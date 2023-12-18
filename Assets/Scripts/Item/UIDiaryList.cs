using Muks.DataBind;
using UnityEngine;
using UnityEngine.UI;

public class UIDiaryList : UIList<Item, ItemField>
{
    private int _currentItemIndex;
    private ItemDatabase _dataBase;

    public Sprite Test;

    private void Awake()
    {
        _dataBase = DatabaseManager.Instance.ItemDatabase;
        GameManager.Instance.Player.Inventory[1].AddById(ItemField.Snack, "I03"); //player가 아이템 하나를 얻음
        
        //Test snack
        for(int i = 0; i < 2; i++)
        {
            _maxCount[i] = _dataBase.ItemCount[i];
            _lists[i] = _dataBase.ItemList[i];
        }

        Init();
    }

    private void StretchImage(RectTransform rectTransform)
    {
        rectTransform.anchorMin = new Vector3(0f, 0f, 0f);
        rectTransform.anchorMax = new Vector3(1f, 1f, 1f);

        rectTransform.offsetMin = new Vector3(0f, 0f, 0f);
        rectTransform.offsetMax = new Vector3(0f, 0f, 0f);

    }

    private void OriginImage(RectTransform rectTransform)
    {
        rectTransform.anchorMin = new Vector3(0.5f, 1f, 0f);
        rectTransform.anchorMax = new Vector3(0.5f, 1f, 1f);

        rectTransform.position = new Vector3(0f, -50f, 0f);
    }

    protected override void GetContent(int index)
    {
        if (_currentField == ItemField.Toy) //친구면 name 
        {
            DataBind.SetTextValue("DiaryDetailName", _lists[(int)_currentField][index].Name);
            OriginImage(_detailView.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>());
        }
        else if(_currentField == ItemField.Snack) //일기장이면 이미지 크게
        {
            StretchImage(_detailView.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>());
        }
        DataBind.SetSpriteValue("DiaryDetailImage", _lists[(int)_currentField][index].Image);
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
