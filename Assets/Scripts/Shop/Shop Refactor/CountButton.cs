using Muks.DataBind;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountButton : MonoBehaviour
{
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private bool _isBuy;

    private int _maxCount;
    private int _price;

    private void OnEnable()
    {
        _countText.text = 1.ToString();
        if (_isBuy)
        {
            _price = int.Parse(DataBind.GetTextValue("ShopBuyItemDetailPrice").Item);
        }
        else
        {
            _price = int.Parse(DataBind.GetTextValue("InventoryDetailPrice").Item);
        }
    }

    void Start()
    {
        if (GetToolItemIdByTName(DataBind.GetTextValue("ShopBuyItemDetailName").Item).StartsWith("ITG")) //도구면 1개만 구매 가능
        {
            _maxCount = 1;
        }
        else
        {
            _maxCount = 10;
        }
        _leftButton.onClick.AddListener(OnClickLeftButton);
        _rightButton.onClick.AddListener(OnClickRightButton);
    }

    private void OnClickLeftButton()
    {
        int result = int.Parse(_countText.text) - 1;
        if(result < 1)
        {
            result = 1;
        }  
        _countText.text = result.ToString();
        _priceText.text = (result * _price).ToString();
    }

    private void OnClickRightButton()
    {
        int result = int.Parse(_countText.text) + 1;
        if (result > _maxCount)
        {
            result = _maxCount;
        }
        _countText.text = result.ToString();
        _priceText.text = (result * _price).ToString();
    }

    private string GetToolItemIdByTName(string name)
    {
        for (int i = 0; i < DatabaseManager.Instance.GetGatheringToolItemList().Count; i++) //shop database에서 아이디 찾기
        {
            if (DatabaseManager.Instance.GetGatheringToolItemList()[i].Name.Equals(name))
            {
                return DatabaseManager.Instance.GetGatheringToolItemList()[i].Id;
            }
        }
        return null;
    }
}
