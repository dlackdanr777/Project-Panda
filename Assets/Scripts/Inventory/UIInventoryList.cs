using Muks.DataBind;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryList : UIList<InventoryItem>
{
    [SerializeField] private Button _arrangeButton;
    [SerializeField] private GameObject _arrangeItem;

    [SerializeField] private DropSlot _dropSlot;
    private int _currentItemIndex;

    //Test
    public Sprite Test;
    public Sprite Test2;

    // Start is called before the first frame update
    private void Start()
    {
        //Test
        GameManager.Instance.Player.Inventory[0].Add(new InventoryItem("0", "n", "d", Test)); //인벤토리에 item add
        GameManager.Instance.Player.Inventory[0].Add(new InventoryItem("0", "n", "d", Test)); //인벤토리에 item add
        GameManager.Instance.Player.Inventory[0].Add(new InventoryItem("1", "n1", "d1", Test2)); //인벤토리에 item add
        GameManager.Instance.Player.Inventory[0].Add(new InventoryItem("2", "n2", "d2", Test2)); //인벤토리에 item add
        
        for (int i = 0; i < GameManager.Instance.Player.Inventory.Length; i++)
        {
            _maxCount[i] = GameManager.Instance.Player.Inventory[i].MaxInventoryItem;
            _lists[i] = GameManager.Instance.Player.Inventory[i].GetInventoryList();//Player에 있는 인벤토리 설정
        }

        Init();
        UpdateListSlots(); //초기 slot update

        _arrangeButton.onClick.AddListener(OnClickArrangeButton);
        _dropSlot.OnUseItem += UIInventoryList_OnUseItem;
    }

    private void OnDisable()
    {
        if(_detailView.activeSelf)
        {
            _detailView.SetActive(false);

        }
        _arrangeButton.onClick.RemoveListener(OnClickArrangeButton);
        _dropSlot.OnUseItem -= UIInventoryList_OnUseItem;
    }

    private void UIInventoryList_OnUseItem()
    {
        UseItem();
    }

    private void OnClickArrangeButton()
    {
        //마우스 따라다니는 이미지 setactive
        MoveItem();

        //Tween 핸드폰 밑으로 내려감
    }

    private void UseItem()
    {
        GameManager.Instance.Player.Inventory[(int)_currentField].RemoveByIndex(_currentItemIndex);
        UpdateListSlots();
        _detailView.SetActive(false);
    }
    private void MoveItem()
    {
        //선택된 아이템의 이미지 보여지기
        _arrangeItem.gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.Player.Inventory[(int)_currentField].GetInventoryList()[_currentItemIndex].Image;
        //이미지 보여지기
        _arrangeItem.SetActive(true);

    }
    protected override void GetContent(int index)
    {
        _currentItemIndex = index;

        DataBind.SetTextValue("InventoryDetailName", _lists[(int)_currentField][index].Name);
        DataBind.SetTextValue("InventoryDetailDescription", _lists[(int)_currentField][index].Description);
        DataBind.SetImageValue("InventoryDetailImage", _lists[(int)_currentField][index].Image);
    }

    private void UpdateListSlots()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(Field)).Length - 1; i++)
        {
            for (int j = 0; j < _maxCount[i]; j++) //현재 player의 인벤토리에 저장된 아이템 갯수
            {
                if (j < GameManager.Instance.Player.Inventory[i].ItemsCount)
                {
                    _spawnPoint[i].GetChild(j).gameObject.SetActive(true);
                    _spawnPoint[i].GetChild(j).GetComponent<Image>().sprite = _lists[(int)_currentField][j].Image;
                    _spawnPoint[i].GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = _lists[(int)_currentField][j].Count.ToString();

                }
                else
                {
                    _spawnPoint[i].GetChild(j).gameObject.SetActive(false);

                }
            }
        }
    }
}