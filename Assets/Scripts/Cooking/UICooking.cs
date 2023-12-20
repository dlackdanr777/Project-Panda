using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICooking : UIView
{
    [SerializeField] private UICookingSlotParent _uiCookingSlotParent;

    [SerializeField] private UICookingSlot[] _uiCookingSlots;

    private Inventory[] _inventory => GameManager.Instance.Player.Inventory;

    public void Start()
    {
        Init(null);
    }

    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);

        _uiCookingSlots = _uiCookingSlotParent.GetComponentsInChildren<UICookingSlot>();

        for (int i = 0, count = _uiCookingSlots.Length; i < count; i++)
        {
            _uiCookingSlots[i].UpdateUI(null);
        }

        for (int i = 0, count = _inventory[0].ItemsCount; i < count; i++)
        {
            _uiCookingSlots[i].UpdateUI(_inventory[0].Items[i]);
        }

    }

    public override void Show()
    {

    }

    public override void Hide()
    {

    }


}
