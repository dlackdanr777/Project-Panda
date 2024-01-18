using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragSticker : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler
{
    [SerializeField] private GameObject _trans;
    [SerializeField] private Button _rotate;
    [SerializeField] private Button _scale;

    private Canvas _canvas;
    private RectTransform _rectTransform;
    private Vector3 _startDragPosition;
    private float _startRotationZ;
    private bool _isRotate;
    private bool _isScale;

    private void Start()
    {
        _canvas = transform.root.GetComponent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _startRotationZ = transform.localEulerAngles.z;

        // EventTrigger ������Ʈ �߰�
        EventTrigger eventTrigger = _rotate.gameObject.AddComponent<EventTrigger>();

        // Pointer Down �̺�Ʈ �߰�
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener(((data) => OnClickTrans(true, false)));
        eventTrigger.triggers.Add(pointerDownEntry);

        _scale.onClick.AddListener(() => OnClickTrans(false, true));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _trans.transform.position = transform.position;
        _startDragPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(_isRotate);
        if( _isRotate)
        {
            Vector3 currentMousePosition = eventData.position;
            Vector3 direction = currentMousePosition - _startDragPosition;

            // ���� ���
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Debug.Log(angle);

            // ������ ���� ������ ������
            float newRotationZ = _startRotationZ + angle;

            // ȸ�� ����
            transform.rotation = Quaternion.Euler(0f, 0f, newRotationZ);
        }
        else if (_isScale)
        {

        }
        else
        {
            Vector2 anchoredPosition = _rectTransform.anchoredPosition + eventData.delta / _canvas.scaleFactor;
            _rectTransform.anchoredPosition = ClampPositionToDropZone(anchoredPosition);
        }
    }

    private Vector2 ClampPositionToDropZone(Vector2 position)
    {
        RectTransform dropZone = transform.parent.GetComponent<RectTransform>();
        // DropZone ������ ��踦 ����Ͽ� �̹��� ��ġ�� ����
        float minX = dropZone.rect.xMin + _rectTransform.rect.width/2;
        float maxX = dropZone.rect.xMax - _rectTransform.rect.width/2;
        float minY = dropZone.rect.yMin + _rectTransform.rect.height/2;
        float maxY = dropZone.rect.yMax - _rectTransform.rect.height/2;

        // ��ġ�� DropZone ���� ���� ����
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        return position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _trans.SetActive(!_trans.activeSelf);
        OnClickTrans(false, false);
    }

    private void OnClickTrans(bool isRotate, bool isScale)
    {
        _isRotate = isRotate;
        _isScale = isScale;
    }
}
