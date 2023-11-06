using System;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventoryList : UIList<InventoryItem>
{
    [SerializeField] private Button _arrangeButton;
    [SerializeField] private SpriteRenderer _selectedImage;
    [SerializeField] private GameObject _arrangeItem;
    private int _currentItemIndex;

    //Test
    public Sprite Test;

    // Start is called before the first frame update
    void Start() 
    {
        for(int i = 0; i < GameManager.Instance.Player.Inventory.Length; i++)
        {
            _maxCount[i] = GameManager.Instance.Player.Inventory[i].MaxInventoryItem;
        }

        //Test
        GameManager.Instance.Player.Inventory[0].Add(new InventoryItem(0, "n", "d", Test)); //인벤토리에 item add
        GameManager.Instance.Player.Inventory[0].Add(new InventoryItem(0, "n", "d", Test)); //인벤토리에 item add
        GameManager.Instance.Player.Inventory[0].Add(new InventoryItem(1, "n1", "d", Test)); //인벤토리에 item add
        GameManager.Instance.Player.Inventory[0].Add(new InventoryItem(2, "n2", "d", Test)); //인벤토리에 item add

        for (int i = 0; i < GameManager.Instance.Player.Inventory[0].ItemsCount; i++)
        {
            Debug.Log(GameManager.Instance.Player.Inventory[0].Items[i].Name);
        }
        _currentField = Field.Toy; //처음에 선택된 장난감으로 초기화
        _lists[(int)_currentField] = GameManager.Instance.Player.Inventory[(int)_currentField].GetInventoryList(); //Player에 있는 인벤토리 설정
        Init();

        _arrangeButton.onClick.AddListener(OnClickArrangeButton);
        
    }

    protected override void SetFieldColorArray()
    {
        _fieldColor[0] = new Color(253 / 255f, 253 / 255f, 150 / 255f, 255 / 255f);
        _fieldColor[1] = new Color(255 / 255f, 192 / 255f, 204 / 255f, 255 / 255f);
    }

    protected override void GetText(int index)
    {
     /*   _currentItemIndex = index;
        Debug.Log(_lists[(int)_currentField][index].Name);
        UIView.SetValue("InventoryDetailName", _lists[(int)_currentField][index].Name);
        UIView.SetValue("InventoryDetailDescription", _lists[(int)_currentField][index].Description);*/
    }

    protected override void UpdateInventorySlots()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(Field)).Length - 1; i++)
        {
            for (int j = 0; j < _maxCount[i]; j++) //현재 player의 인벤토리에 저장된 아이템 갯수
            {
                if(j < GameManager.Instance.Player.Inventory[i].ItemsCount)
                {
                    _spawnPoint[i].GetChild(j).gameObject.SetActive(true);
                    _spawnPoint[i].GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = _lists[(int)_currentField][j].Count.ToString();

                }
                else
                {
                    _spawnPoint[i].GetChild(j).gameObject.SetActive(false);

                }
            }
        }
    }
    private void OnClickArrangeButton()
    {
        UseItem();
        _detailView.SetActive(false);
        
        MoveItem();
        //마우스 따라다니는 이미지 setactive
        

    }

    private void UseItem()
    {
        GameManager.Instance.Player.Inventory[(int)_currentField].RemoveByIndex(_currentItemIndex);
        UpdateInventorySlots();
    }

    private void MoveItem()
    {
        //선택된 아이템의 이미지 보여지기
        _selectedImage.sprite = GameManager.Instance.Player.Inventory[(int)_currentField].GetInventoryList()[_currentItemIndex].Image;
        //이미지 보여지기
        _arrangeItem.SetActive(true);

    }
    
}
