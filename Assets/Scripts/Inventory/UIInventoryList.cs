using Muks.DataBind;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryList : UIList<InventoryItem, ItemField>
{
    [SerializeField] private Button _arrangeButton;
    [SerializeField] private Button _sellButton;
    [SerializeField] private GameObject _arrangeItem;
    [SerializeField] private GameObject _soldPanel;

    private int _currentItemIndex;

    //Test
    public Sprite Test;
    public Sprite Test2;

    private void Awake()
    {

        //Test
        GameManager.Instance.Player.Inventory[0].Add(new Item("0", "n", "d", 100, Test)); //인벤토리에 item add
        GameManager.Instance.Player.Inventory[0].Add(new Item("0", "n", "d", 100, Test)); //인벤토리에 item add
        GameManager.Instance.Player.Inventory[0].Add(new Item("1", "n1", "d1", 200, Test2)); //인벤토리에 item add
        GameManager.Instance.Player.Inventory[0].Add(new Item("2", "n2", "d2", 300, Test2)); //인벤토리에 item add
        
        //미리 생성 => spawn 계속하면 안좋음
        for (int i = 0; i < GameManager.Instance.Player.Inventory.Length; i++)
        {
            _maxCount[i] = GameManager.Instance.Player.Inventory[i].MaxInventoryItem;
            _lists[i] = GameManager.Instance.Player.Inventory[i].GetInventoryList();//Player에 있는 인벤토리 설정 -> 변경될 때마다 이벤트로 ui 변경해줘야 함
        }
        Init();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _arrangeButton.GetComponent<DragAndDrop>().OnUseItem += UIInventoryList_OnUseItem;
        _arrangeButton.GetComponent<DragAndDrop>().DontUseItem += UIInventoryList_DontUseItem;

        _sellButton.onClick.AddListener(OnClickSellButton);
    }

    private void UIInventoryList_OnUseItem() //아이템 
    {
        UseItem();    
    }

    private void UIInventoryList_DontUseItem() //상세 설명창 닫음
    {
        _detailView.SetActive(false);
    }

    private void UseItem()
    {
        GameManager.Instance.Player.Inventory[(int)_currentField].RemoveByIndex(_currentItemIndex);
        UpdateListSlots();
        //상세 설명창 닫음
        _detailView.SetActive(false);
    }

    private void UpdateList()
    {
        for (int i = 0; i < GameManager.Instance.Player.Inventory.Length; i++)
        {
            _lists[i] = GameManager.Instance.Player.Inventory[i].GetInventoryList();//Player에 있는 인벤토리 설정 -> 변경될 때마다 이벤트로 ui 변경해줘야 함
        }
    }

    //Sell
    private void OnClickSellButton()
    {
        if (GameManager.Instance.Player.GainBamboo(_lists[(int)_currentField][_currentItemIndex].Price))
        {
            StartCoroutine(ItemSold());

        }
    }

    private IEnumerator ItemSold()
    {
        _detailView.SetActive(false);
        _soldPanel.SetActive(true);

        GameManager.Instance.Player.Inventory[(int)_currentField].RemoveByIndex(_currentItemIndex);
        UpdateListSlots();

        yield return new WaitForSeconds(1.0f);

        _soldPanel.SetActive(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (_soldPanel.activeSelf)
        {
            _soldPanel.SetActive(false);
        }

        _arrangeButton.GetComponent<DragAndDrop>().DontUseItem -= UIInventoryList_DontUseItem;
        _arrangeButton.GetComponent<DragAndDrop>().OnUseItem -= UIInventoryList_OnUseItem;

    }

    protected override void GetContent(int index)
    {
        _currentItemIndex = index;

        DataBind.SetTextValue("InventoryDetailName", _lists[(int)_currentField][index].Name);
        DataBind.SetTextValue("InventoryDetailDescription", _lists[(int)_currentField][index].Description);
        DataBind.SetSpriteValue("InventoryDetailImage", _lists[(int)_currentField][index].Image);
        //배치아이템 data bind
        DataBind.SetSpriteValue("ArrangeItemSprite", GameManager.Instance.Player.Inventory[(int)_currentField].GetInventoryList()[_currentItemIndex].Image);

    }

    protected override void UpdateListSlots()
    {
        UpdateList();

        for (int j = 0; j < _maxCount[(int)_currentField]; j++) //현재 player의 인벤토리에 저장된 아이템 갯수
        {
            if (j < GameManager.Instance.Player.Inventory[(int)_currentField].ItemsCount)
            {
                _spawnPoint[(int)_currentField].GetChild(j).gameObject.SetActive(true);
                _spawnPoint[(int)_currentField].GetChild(j).GetComponent<Image>().sprite = _lists[(int)_currentField][j].Image;
                _spawnPoint[(int)_currentField].GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = _lists[(int)_currentField][j].Count.ToString();

            }
            else
            {

                _spawnPoint[(int)_currentField].GetChild(j).gameObject.SetActive(false);


            }
        }

    }
}