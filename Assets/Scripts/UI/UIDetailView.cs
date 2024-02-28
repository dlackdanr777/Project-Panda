using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>������ ������ �޾� ����ִ� UI Ŭ����</summary>
public class UIDetailView : MonoBehaviour
{
    [SerializeField] private Image _itemImage;

    [SerializeField] private TextMeshProUGUI _nameText;

    [SerializeField] private TextMeshProUGUI _descriptionText;

    [SerializeField] private Button _exitButton;


    public void Init(UnityAction onButtonClicked = null)
    {
        if(onButtonClicked != null)
            _exitButton.onClick.AddListener(onButtonClicked);

        _exitButton.onClick.AddListener(() => gameObject.SetActive(false));
    }


    public void Show(Item item)
    {
        gameObject.SetActive(true);
        _itemImage.sprite = item.Image;
        _nameText.text = item.Name;
        _descriptionText.text = item.Description;
    }
}
