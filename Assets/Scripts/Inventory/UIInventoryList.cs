using Muks.DataBind;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryList : UIList<InventoryItem>
{
    [SerializeField] private Button _arrangeButton;
    [SerializeField] private GameObject _arrangeItem;

    private int _currentItemIndex;

    //Test
    public Sprite Test;
    public Sprite Test2;

    private void Awake()
    {

        //Test
        GameManager.Instance.Player.Inventory[0].Add(new Item("0", "n", "d", Test)); //�κ��丮�� item add
        GameManager.Instance.Player.Inventory[0].Add(new Item("0", "n", "d", Test)); //�κ��丮�� item add
        GameManager.Instance.Player.Inventory[0].Add(new Item("1", "n1", "d1", Test2)); //�κ��丮�� item add
        GameManager.Instance.Player.Inventory[0].Add(new Item("2", "n2", "d2", Test2)); //�κ��丮�� item add
        
        for (int i = 0; i < GameManager.Instance.Player.Inventory.Length; i++)
        {
            _maxCount[i] = GameManager.Instance.Player.Inventory[i].MaxInventoryItem;
            _lists[i] = GameManager.Instance.Player.Inventory[i].GetInventoryList();//Player�� �ִ� �κ��丮 ���� -> ����� ������ �̺�Ʈ�� ui ��������� ��
        }
        Init();
    }

    // Start is called before the first frame update
    private void Start()
    {

        _arrangeButton.GetComponent<DragAndDrop>().OnUseItem += UIInventoryList_OnUseItem;
        _arrangeButton.GetComponent<DragAndDrop>().DontUseItem += UIInventoryList_DontUseItem;
    }

    private void OnDisable()
    {
        if(_detailView.activeSelf)
        {
            _detailView.SetActive(false);

        }

        //DataBind.SetTextValue("InventoryDetailName", _lists[(int)_currentField][index].Name);
        //DataBind.SetTextValue("InventoryDetailDescription", _lists[(int)_currentField][index].Description);
        //DataBind.SetSpriteValue("InventoryDetailImage", _lists[(int)_currentField][index].Image);
        _arrangeButton.GetComponent<DragAndDrop>().DontUseItem -= UIInventoryList_DontUseItem;
        _arrangeButton.GetComponent<DragAndDrop>().OnUseItem -= UIInventoryList_OnUseItem;
    }

    private void UIInventoryList_OnUseItem() //������ 
    {
        UseItem();    
    }

    private void UIInventoryList_DontUseItem() //�� ����â ����
    {
        _detailView.SetActive(false);
    }

    private void UseItem()
    {
        GameManager.Instance.Player.Inventory[(int)_currentField].RemoveByIndex(_currentItemIndex);
        UpdateListSlots();
        //�� ����â ����
        _detailView.SetActive(false);
    }


    
    protected override void GetContent(int index)
    {
        _currentItemIndex = index;

        DataBind.SetTextValue("InventoryDetailName", _lists[(int)_currentField][index].Name);
        DataBind.SetTextValue("InventoryDetailDescription", _lists[(int)_currentField][index].Description);
        DataBind.SetImageValue("InventoryDetailImage", _lists[(int)_currentField][index].Image);
        //��ġ������ data bind
        DataBind.SetImageValue("ArrangeItemSprite", GameManager.Instance.Player.Inventory[(int)_currentField].GetInventoryList()[_currentItemIndex].Image);

    }

    protected override void UpdateListSlots()
    {
        for (int j = 0; j < _maxCount[(int)_currentField]; j++) //���� player�� �κ��丮�� ����� ������ ����
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