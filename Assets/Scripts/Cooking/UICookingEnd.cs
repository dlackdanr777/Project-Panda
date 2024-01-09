using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICookingEnd : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    [SerializeField] private Button _okButton;

    public void Init(UnityAction onButtonClicked)
    {
        _okButton.onClick.AddListener(onButtonClicked);
        _okButton.onClick.AddListener(() => gameObject.SetActive(false));
        gameObject.SetActive(false);
    }

    public void Show(Item item)
    {
        _itemImage.sprite = item.Image;
        _nameText.text = item.Name;
        _descriptionText.text = item.Description;
        gameObject.SetActive(true);
    }


}
