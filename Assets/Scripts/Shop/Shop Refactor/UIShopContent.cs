using Muks.DataBind;
using Shop;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopContent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private UIShopSlot _slotPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private UIShopBuyDetailView _buyDetailView;


    [Header("Setting")]
    [SerializeField] private string _mapId;

    private List<Item> _shopItem = new List<Item>(); //아직 상점 데이터베이스가 없어서 툴로 사용
    private List<Item> _buyItemList = new List<Item>();
    private List<UIShopSlot> _slotList = new List<UIShopSlot>();
    private int _currentIndex;


    private void Awake()
    {
        DataBind.SetButtonValue("ShopExitButton", OnExitButtonClicked);
        _buyDetailView.Init();
        _buyDetailView.gameObject.SetActive(false);

        LoadingSceneManager.OnLoadSceneHandler += OnSceneChanged;
        UIBuyView.OnBuyCompleteHandler += UpdateSoldOut;
        UISellView.OnSellCompleteHandler += UpdateSoldOut;
    }


    void Start()
    {
        _shopItem.AddRange(DatabaseManager.Instance.ItemDatabase.ItemBugList);
        _shopItem.AddRange(DatabaseManager.Instance.ItemDatabase.ItemFishList);
        _shopItem.AddRange(DatabaseManager.Instance.ItemDatabase.ItemFruitList);
        _shopItem.AddRange(DatabaseManager.Instance.ItemDatabase.ItemToolList);
        CreateSlots();
    }


    private void CreateSlots()
    {
        int index = 0;

        for (int i = 0; i < _shopItem.Count; i++)
        {

            if (string.IsNullOrWhiteSpace(_mapId))
            {
                continue;
            }

            if (_shopItem[i].Map.Equals(_mapId))
            {
                UIShopSlot slot = Instantiate(_slotPrefab, _spawnPoint);
                slot.Init();

                int itemIndex = index;
                slot.AddOnButtonClicked(() => OnSlotClicked(itemIndex));
                slot.SetItemImage(_shopItem[i].Image);
                slot.SetPriceText(_shopItem[i].Price.ToString());
                slot.transform.localScale = Vector3.one;

                _slotList.Add(slot);
                _buyItemList.Add(_shopItem[i]);
                index++;
            }
        }
    }


    private void OnSlotClicked(int index)
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
        GetContent(index);
    }


    private void GetContent(int index)
    {
        _currentIndex = index;
        _buyDetailView.Show(_buyItemList[index]);
    }



    private void UpdateSoldOut()
    {
        //soldout
        //보유중인 도구 아이템을 확인해 보유중이면 매진 처리를 한다.
        List<InventoryItem> possessedItemList = new List<InventoryItem>();
        possessedItemList.AddRange(GameManager.Instance.Player.ToolItemInventory[(int)ToolItemType.GatheringTool].GetItemList());

        for (int i = 0, count = _buyItemList.Count; i < count; i++)
        {
            if (possessedItemList.Find(x => x.Id == _buyItemList[i].Id) == null)
            {
                _slotList[i].NotSoldOut();
                continue;
            }
                
            _slotList[i].SoldOut();
        }
    }


    private void OnExitButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
        LoadingSceneManager.LoadScene("24_01_09_Integrated");
    }


    private void OnSceneChanged()
    {
        UIBuyView.OnBuyCompleteHandler -= UpdateSoldOut;
        UISellView.OnSellCompleteHandler -= UpdateSoldOut;
        LoadingSceneManager.OnLoadSceneHandler -= OnSceneChanged;

    }

}
