using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragSticker : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler
{
    [SerializeField] private GameObject _trans;

    private Canvas _canvas;
    private RectTransform _rectTransform;

    private void Start()
    {
        _canvas = transform.root.GetComponent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _trans.transform.position = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 anchoredPosition = _rectTransform.anchoredPosition + eventData.delta / _canvas.scaleFactor;
        _rectTransform.anchoredPosition = ClampPositionToDropZone(anchoredPosition);
    }

    private Vector2 ClampPositionToDropZone(Vector2 position)
    {
        RectTransform dropZone = transform.parent.GetComponent<RectTransform>();
        // DropZone 영역의 경계를 계산하여 이미지 위치를 제한
        float minX = dropZone.rect.xMin + _rectTransform.rect.width * _rectTransform.localScale.x /2;
        float maxX = dropZone.rect.xMax - _rectTransform.rect.width * _rectTransform.localScale.x /2;
        float minY = dropZone.rect.yMin + _rectTransform.rect.height * _rectTransform.localScale.y /2;
        float maxY = dropZone.rect.yMax - _rectTransform.rect.height * _rectTransform.localScale.y /2;

        // 위치를 DropZone 영역 내로 제한
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        return position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _trans.SetActive(!_trans.activeSelf);
    }
}
