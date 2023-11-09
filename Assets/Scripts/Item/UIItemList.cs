using Muks.DataBind;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIItemList : UIList<Item>
{
    private int _currentItemIndex;
    private Database_Ssun _dataBase;

    //Test
    public Sprite TestImage;
    void Start()
    {
        //Test
        _dataBase = Database_Ssun.Instance;
        _maxCount[2] = _dataBase.DataSnack.Count;

        //for (int i = 0; i < _maxCount.Length; i++)
        //{
        //    for (int j = 0; j < _maxCount[i]; j++)
        //    {
        //        _lists[i].Add(new Item(_dataBase.DataSnack[j]["Id"].ToString(),
        //            _dataBase.DataSnack[j]["Name"].ToString(),
        //            _dataBase.DataSnack[j]["Description"].ToString(),
        //            null));
        //    }
        //}

        GameManager.Instance.Player.Inventory[1].Add(new Item(_dataBase.DataSnack[2]["Id"].ToString(),
                    _dataBase.DataSnack[2]["Name"].ToString(),
                    _dataBase.DataSnack[2]["Description"].ToString(),
                    null));

        Init();
    }

    private void OnEnable()
    {
        CheckReceivedItem();
    }

    private void CheckReceivedItem()
    {
        //for (int i = 0; i < _maxCount.Length; i++)
        //{
        //    for (int j = 0; j < _maxCount[i]; j++)
        //    {
        //        if (_lists[i][j].IsReceived)
        //        {
        //            _spawnPoint[i].GetChild(j).GetComponent<Button>().interactable = true;
        //        }
        //    }
        //}
        for (int j = 0; j < _maxCount[2]; j++)
        {
            if (_lists[2][j].IsReceived)
            {
                _spawnPoint[2].GetChild(j).GetComponent<Button>().interactable = true;
                _spawnPoint[2].GetChild(j).GetComponent<Image>().sprite = _lists[2][j].Image;

            }
        }
    }
    protected override void GetContent(int index)
    {
        _currentItemIndex = index;

        DataBind.SetTextValue("ItemDetailName", _lists[(int)_currentField][index].Name);
        DataBind.SetTextValue("ItemDetailDescription", _lists[(int)_currentField][index].Description);
        DataBind.SetImageValue("ItemDetailImage", _lists[(int)_currentField][index].Image);
    }

    
}
