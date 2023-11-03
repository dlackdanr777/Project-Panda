using System.Collections.Generic;
using UnityEngine;

public class UIInventoryList : UIList<InventoryItem>
{
    public override void SetFieldColorArray()
    {
        _fieldColor[0] = new Color(253 / 255f, 253 / 255f, 150 / 255f, 255 / 255f);
        _fieldColor[1] = new Color(255 / 255f, 192 / 255f, 204 / 255f, 255 / 255f);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        CreateSlots();
    }

    void OnEnable()
    {
        //Test
        GameManager.Instance.Player.ToyInventory.Add(new InventoryItem(0, "n", "d", null)); //Player에 있는 인벤토리 설정
        _lists[(int)_currentField] = GameManager.Instance.Player.ToyInventory.Items; //Player에 있는 인벤토리 설정

    }

}
