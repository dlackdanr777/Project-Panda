using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private GameObject _dropItemPf;

    private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Transform _oldParent;
    private Vector3 _oldPosition;

    private void Awake()
    {
        _canvas = transform.root.GetComponent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _oldParent = transform.parent;
        _oldPosition = transform.position;
        _canvasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.parent = transform.root; //맨 앞으로 보내기
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        DropObjectInWorld(eventData); //현재 프리펩의 ray감지못하도록 먼저 실행
        _canvasGroup.blocksRaycasts = true;
        transform.parent = _oldParent;
        transform.position = _oldPosition;

    }

    private void DropObjectInWorld(PointerEventData eventData)
    {
        if (_dropItemPf == null)
        {
            return;
        }

        // UI에서 드래그한 위치를 World 좌표로 변환
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 0));
        worldPosition.z = 0;

        //Vector3 rayOrigin = worldPosition;
        //Vector2 rayDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - worldPosition; //카메라에서 worldPosition을 향햐도록
        //rayDirection.Normalize();

        //RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection);

        GameObject dropObject = Instantiate(_dropItemPf, worldPosition, Quaternion.identity);

        Collider2D dropCollider = dropObject.GetComponent<Collider2D>();
        if(dropCollider == null)
        {
            Destroy(dropObject);
            return;
        }

        //박스 레이캐스트 크기 설정
        Vector2 boxSize = dropCollider.bounds.size;


        //Vector2 rayOrigin = worldPosition;
        //Vector2 boxDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - worldPosition;

        //Vector2 boxRayStart = rayOrigin;

        Collider2D[] overlaps = Physics2D.OverlapBoxAll(worldPosition, boxSize, 0f);
        bool isFullyInsideRoom = false;
        //Debug.DrawRay(boxRayStart, boxDirection, Color.red, 2f);


        bool isCollideingWithRoom = false;
        foreach (var overlap in overlaps) //모든 충돌 검사
        {
            Debug.Log(overlap);
            //if(overlap == dropCollider ) //나 자신과 충돌
            //{
            //    continue;
            //}
            //if (overlap != null && overlap.CompareTag("Room") && overlap == dropCollider && overlap.CompareTag("DropZone"))//방태그와 충돌
            //{
            //    isCollideingWithRoom = true;
            //}
            //else if(overlap == null || !overlap.CompareTag("Room"))
            //{
            //    isCollideingWithRoom = false;
            //    break;

            //}
            if(overlap.CompareTag("Room") && overlap != dropCollider)
            {

                if (!dropCollider.bounds.Contains(overlap.bounds.min) || !dropCollider.bounds.Contains(overlap.bounds.max))
                {
                    isFullyInsideRoom = true;
                    break;
                }
            }
        }

        //나 자신과 충돌 -> 무조건
        //나 자신을 제외한 다른 충돌 검사 -> room -> instantiate

        if (isFullyInsideRoom)
        {
            Debug.Log("가구 배치!");
        }
        else
        {
            Debug.Log("놓을 수 없는 공간");
            Destroy(dropObject);
        }


        //if (hit.collider.CompareTag("Room"))
        //{

        //    Debug.Log("Room tag를 가지고 있는 객체");
        //    Instantiate(_dropItemPf, hit.point, Quaternion.identity);

        //}
        //else
        //{
        //    Debug.Log("놓을 수 없는 공간!");
        //}

    }
}