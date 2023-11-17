using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopList : UIList<Item>
{
    [SerializeField] private Button _purchaseButton;
    [SerializeField] private GameObject _purchasedPanel;
    private int _currentItemIndex;
    private Database_Ssun _dataBase;

    private void Awake()
    {
        _dataBase = Database_Ssun.Instance;
        GameManager.Instance.Player.Inventory[1].AddById(Field.Snack, "I03"); //player가 아이템 하나를 얻음

        //Test snack
        for (int i = 0; i < 2; i++)
        {
            _maxCount[i] = _dataBase.ItemCount[i];
            _lists[i] = _dataBase.ItemList[i];
        }

        Init();
    }

    private void Start()
    {
        _purchaseButton.onClick.AddListener(OnClickPurchaseButton);
    }

    private void OnClickPurchaseButton()
    {
        StartCoroutine(ItemPurchased());
    }

    IEnumerator ItemPurchased()
    {
        _detailView.SetActive(false);
        _purchasedPanel.SetActive(true);

        GameManager.Instance.Player.Inventory[(int)_currentField].AddById(_currentField, _dataBase.ItemList[(int)_currentField][_currentItemIndex].Id);

        yield return new WaitForSeconds(1.0f);

        _purchasedPanel.SetActive(false);
    }

    protected override void GetContent(int index)
    {
        _currentItemIndex = index;

        DataBind.SetTextValue("ShopItemDetailName", _lists[(int)_currentField][index].Name);
        DataBind.SetTextValue("ShopItemDetailDescription", _lists[(int)_currentField][index].Description);
        DataBind.SetSpriteValue("ShopItemDetailImage", _lists[(int)_currentField][index].Image);
    }

    protected override void UpdateListSlots()
    {
        for (int j = 0; j < _maxCount[(int)_currentField]; j++)
        {          
            _spawnPoint[(int)_currentField].GetChild(j).GetComponent<Image>().sprite = _lists[(int)_currentField][j].Image;
            
        }
    }

    protected override void OnClickSlot(int index)
    {
        base.OnClickSlot(index);
        if(_currentField == Field.Toy || _currentField == Field.Snack)
        {
            _purchaseButton.gameObject.SetActive(true);
        }
    }
}
