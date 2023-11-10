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
        GameManager.Instance.Player.Inventory[0].Add(new InventoryItem("0", "n", "d", Test)); //�κ��丮�� item add
        GameManager.Instance.Player.Inventory[0].Add(new InventoryItem("0", "n", "d", Test)); //�κ��丮�� item add
        GameManager.Instance.Player.Inventory[0].Add(new InventoryItem("1", "n1", "d1", Test2)); //�κ��丮�� item add
        GameManager.Instance.Player.Inventory[0].Add(new InventoryItem("2", "n2", "d2", Test2)); //�κ��丮�� item add
        
        for (int i = 0; i < GameManager.Instance.Player.Inventory.Length; i++)
        {
            _maxCount[i] = GameManager.Instance.Player.Inventory[i].MaxInventoryItem;
            _lists[i] = GameManager.Instance.Player.Inventory[i].GetInventoryList();//Player�� �ִ� �κ��丮 ���� -> ����� ������ �̺�Ʈ�� ui ��������� ��
        }

        Init();
        UpdateListSlots(); //�ʱ� slot update


        //�̺�Ʈ Ʈ���� pointenter
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
        Debug.Log("Ȯ����");
        UseItem();    
    }

    private void UIInventoryList_DontUseItem()
    {
        _detailView.SetActive(false);
    }

    private void OnClickArrangeButton()
    {
        //���콺 ����ٴϴ� �̹��� setactive
        //MoveItem(); //DragItem���� ����

        Debug.Log("����Ȯ�� arrangeButton");
        //Tween �ڵ��� ������ ������
        
    }

    private void OnPointerEnterArrangeButton(PointerEventData data)
    {
        //��ġ ��ư�� �հ����� �÷��θ� �ش� �������� ���̵���
        DataBind.SetImageValue("ArrangeItemSprite", GameManager.Instance.Player.Inventory[(int)_currentField].GetInventoryList()[_currentItemIndex].Image);
        
    }

    private void UseItem()
    {
        GameManager.Instance.Player.Inventory[(int)_currentField].RemoveByIndex(_currentItemIndex);
        UpdateListSlots();
        //�� ����â ����
        _detailView.SetActive(false);
    }

    private void MoveItem()
    {
        //���õ� �������� �̹��� ��������
        _arrangeItem.gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.Player.Inventory[(int)_currentField].GetInventoryList()[_currentItemIndex].Image;
        //�̹��� ��������
        _arrangeItem.SetActive(true);
    }

    protected override void GetContent(int index)
    {
        _currentItemIndex = index;

        DataBind.SetTextValue("InventoryDetailName", _lists[(int)_currentField][index].Name);
        DataBind.SetTextValue("InventoryDetailDescription", _lists[(int)_currentField][index].Description);
        DataBind.SetImageValue("InventoryDetailImage", _lists[(int)_currentField][index].Image);
        //��ġ ��ư�� �հ����� �÷��θ� �ش� �������� ���̵���
        DataBind.SetImageValue("ArrangeItemSprite", GameManager.Instance.Player.Inventory[(int)_currentField].GetInventoryList()[_currentItemIndex].Image);

    }

    private void UpdateListSlots()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(Field)).Length - 1; i++)
        {
            for (int j = 0; j < _maxCount[i]; j++) //���� player�� �κ��丮�� ����� ������ ����
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