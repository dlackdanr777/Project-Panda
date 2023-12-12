using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Transform _oldParent;
    private Vector2 _originalLocalPos;
    private Vector3 _oldPosition;
    private GameObject _worldObject;

    private Vector3 _originalWorldPos;
    private Vector2 _originalMousePos;
    private Vector3 _offset;


    private void Awake()
    {
        _canvas = transform.root.GetComponent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _originalLocalPos = _rectTransform.localPosition;
        _worldObject = GameObject.FindGameObjectWithTag("DropZone");

        // 초기 World 좌표와 마우스 위치 저장
        _originalWorldPos = transform.position;
        _originalMousePos = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _oldParent = transform.parent;
        _oldPosition = transform.position;
        _canvasGroup.blocksRaycasts = false;

        transform.parent = transform.root; //맨 위로 가도록

        // 드래그가 시작될 때의 처리
        Vector3 mousePos = GetMousePosInWorld(eventData.position);
        _offset = _rectTransform.position - mousePos;
    }
    public void OnDrag(PointerEventData eventData)
    {
        //_rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;

        //Vector2 viewportPosition = Camera.main.WorldToViewportPoint(_worldObject.transform.position);

        //Vector2 worldObject_ScreenPosition = new Vector2(
        //((viewportPosition.x * _rectTransform.sizeDelta.x) - (_rectTransform.sizeDelta.x * 0.5f)),
        //((viewportPosition.y * _rectTransform.sizeDelta.y) - (_rectTransform.sizeDelta.y * 0.5f)));

        ////now you can set the position of the ui element
        //_rectTransform.anchoredPosition = worldObject_ScreenPosition;

        MoveWithMouse(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        transform.parent = _oldParent;
        transform.position = _oldPosition;
    }

    private void MoveWithMouse(PointerEventData eventData)
    {

        // 현재 마우스 위치를 World 좌표로 변환
        Vector3 mousePos = GetMousePosInWorld(eventData.position);

        // World 좌표로 변환된 마우스 위치를 기반으로 UI를 이동
        _rectTransform.position = mousePos + _offset;
    }

    private Vector3 GetMousePosInWorld(Vector3 screenPos)
    {
        // 마우스의 스크린 위치를 World 좌표로 변환
        return Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, _rectTransform.position.z));
    }
}
