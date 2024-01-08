using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static Action<GameObject> OnDropEvent = delegate { };

    [SerializeField] private GameObject _dropItemPf;
    
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Transform _oldParent;
    private Vector3 _oldPosition;
    private string _id;

    private void Awake()
    {
        _canvas = transform.root.GetComponent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _id = transform.Find("Id").GetComponent<TextMeshProUGUI>().text;
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

        _canvasGroup.blocksRaycasts = true;
        transform.parent = _oldParent;
        transform.position = _oldPosition;
        DropObjectInWorld(eventData);

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

        CheckCollider(worldPosition);

 
    }

    private void CheckCollider(Vector3 worldPosition)
    {
        GameObject dropObject = Instantiate(_dropItemPf, worldPosition, Quaternion.identity);

        Collider2D dropCollider = dropObject.GetComponent<Collider2D>();
        if (dropCollider == null)
        {
            Destroy(dropObject);
            return;
        }

        //박스 레이캐스트 크기 설정
        Vector2 boxSize = dropCollider.bounds.size;

        Collider2D[] overlaps = Physics2D.OverlapBoxAll(worldPosition, boxSize, 0f);
        bool isFullyInsideRoom = false;


        foreach (var overlap in overlaps) //모든 충돌 검사
        {

            if (overlap.CompareTag("Item") && overlap.gameObject != dropCollider.gameObject) //현재 아이템이 아니면서 생성된 아이템이 있으면
            {
                break;
            }

            if (overlap.CompareTag("Room") && overlap.gameObject != dropCollider.gameObject) //room tag를 가지고, 현재 오브젝트가 아니면
            {
                if (overlap.bounds.Contains(dropCollider.bounds.min) && overlap.bounds.Contains(dropCollider.bounds.max)) //생성되는 물체의 최소 지점과 최대 지점이 spawn될 위치의 콜라이더가 포합하는지
                {
                    Debug.Log(overlap.name);
                    isFullyInsideRoom = true;
                    dropObject.transform.SetParent(overlap.transform);

                    dropObject.transform.Find("Id").GetComponent<TextMeshProUGUI>().text = _id;
                    Debug.Log(_id);
                    OnDropEvent?.Invoke(dropObject);
                    //해당 내용 저장해서 로컬로 자료 빼기
                    break;


                }
            }
        }

        if (isFullyInsideRoom)
        {
            Debug.Log("가구 배치!");
        }
        else
        {
            Debug.Log("놓을 수 없는 공간");
            Destroy(dropObject);
        }
    }
}