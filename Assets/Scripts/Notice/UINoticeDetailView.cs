using BackEnd.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>�������� �� ����</summary>
public class UINoticeDetailView : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _contentsText;
    [SerializeField] private Image _image;
    [SerializeField] private Button _exitButton;


    private float _tmpImageWidth;


    public void Init()
    {
        _exitButton.onClick.AddListener(Hide);

        _contentsText.transform.localScale = Vector3.one;
        _image.transform.localScale = Vector3.one;
        _tmpImageWidth = _image.rectTransform.sizeDelta.x;
        gameObject.SetActive(false);
    }


    public void Show(Notice notice)
    {
        gameObject.SetActive(true);
        _titleText.text = notice.Title;
        _contentsText.text = notice.Content;

        if (notice.Sprite == null)
        {
            _image.gameObject.SetActive(false);
            return;
        }

        _image.gameObject.SetActive(true);

        //�̹��� ������ �°� Image ������Ʈ�� ����� �����Ѵ�.
        float heightMul = (float)notice.Sprite.textureRect.height / (float)notice.Sprite.textureRect.width;
        Vector2 sizeDelta = _image.rectTransform.sizeDelta;
        _image.rectTransform.sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y * heightMul);
        _image.sprite = notice.Sprite;
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
