using Muks.DataBind;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
            _lists[i] = GameManager.Instance.Player.Inventory[i].GetInventoryList();//Player에 있는 인벤토리 설정 -> 변경될 때마다 이벤트로 ui 변경해줘야 함
        }

        Init();
        UpdateListSlots(); //초기 slot update


        //이벤트 트리거 pointenter
        //EventTrigger eventTrigger = _arrangeButton.gameObject.AddComponent<EventTrigger>();
        //EventTrigger.Entry entryPointerEnter = new EventTrigger.Entry();
        //entryPointerEnter.eventID = EventTriggerType.PointerEnter;
        //entryPointerEnter.callback.AddListener((data) => { OnPointerEnterArrangeButton((PointerEventData)data); });
        //eventTrigger.triggers.Add(entryPointerEnter);

        _arrangeButton.GetComponent<DragAndDrop>().OnUseItem += UIInventoryList_OnUseItem;
        _arrangeButton.GetComponent<DragAndDrop>().DontUseItem += UIInventoryList_DontUseItem;
    }

    private void OnDisable()
    {
        if(_detailView.activeSelf)
        {
            _detailView.SetActive(false);

        }
        //_arrangeButton.onClick.RemoveListener(OnClickArrangeButton);
        _arrangeButton.GetComponent<DragAndDrop>().OnUseItem -= UIInventoryList_OnUseItem;
    }

    private void UIInventoryList_OnUseItem()
    {
        Debug.Log("확ㅇ니");
        UseItem();    
    }

    private void UIInventoryList_DontUseItem()
    {
        _detailView.SetActive(false);
    }

    private void OnClickArrangeButton()
    {
        //마우스 따라다니는 이미지 setactive
        //MoveItem(); //DragItem으로 변경

        Debug.Log("실행확인 arrangeButton");
        //Tween 핸드폰 밑으로 내려감
        
    }

    private void OnPointerEnterArrangeButton(PointerEventData data)
    {
        //배치 버튼에 손가락을 올려두면 해당 아이템이 보이도록
        DataBind.SetImageValue("ArrangeItemSprite", GameManager.Instance.Player.Inventory[(int)_currentField].GetInventoryList()[_currentItemIndex].Image);
        
    }

    private void UseItem()
    {
        GameManager.Instance.Player.Inventory[(int)_currentField].RemoveByIndex(_currentItemIndex);
        UpdateListSlots();
        //상세 설명창 닫음
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
        //배치 버튼에 손가락을 올려두면 해당 아이템이 보이도록
        DataBind.SetImageValue("ArrangeItemSprite", GameManager.Instance.Player.Inventory[(int)_currentField].GetInventoryList()[_currentItemIndex].Image);

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