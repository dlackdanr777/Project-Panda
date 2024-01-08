using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    public Transform _dropItemSpawnPoint;
    
    [SerializeField] private GameObject _itemDropPopup;
    [SerializeField] private GameObject _itemPf;

    private int _currentItemIndex;
    private Image _selectImage;
    private Vector3 _worldPosition;


    //Test 
    public Sprite Image;

    private void Start()
    {

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, transform.position, Camera.main, out var offset))
        {
            _worldPosition = Camera.main.ScreenToWorldPoint(transform.position);
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<DragDrop>() != null) //드래그를 가지고 있는 객체만 drop
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            //Test
            eventData.pointerDrag.GetComponent<Image>().sprite = Image;


            //이미지로 적용 -> 2d 오브젝트로 변경
            //카메라 위치 변경해서 해당 위치에 스폰

            if (GetComponent<Image>().sprite == null)
            {
                _itemDropPopup.SetActive(true); //popup 띄움

                _selectImage = eventData.pointerDrag.GetComponent<Image>();
                


                //UI
                GetComponent<Image>().sprite = _selectImage.sprite;
                transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = eventData.pointerDrag.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text; //id
                
                //Prefab
                _itemPf.GetComponent<SpriteRenderer>().sprite = _selectImage.sprite; //이미지
                
                _selectImage.sprite = null;

                _currentItemIndex =  FindChildIndex();
                transform.parent.GetComponent<DropZone_Old>().CurrentItemIndex = _currentItemIndex;

                ChangeAlpha(GetComponent<Image>(), 1f);

                Vector3 targetPosition = transform.position;
                
                //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(targetPosition);

                //GameObject spawnItem = Instantiate(_itemPf);
                //spawnItem.transform.SetParent(_dropItemSpawnPoint.GetChild(_currentItemIndex));
                //spawnItem.transform.position = new Vector3(_worldPosition.x, _worldPosition.y, 0);
            }


        }
    }
    private void ChangeAlpha(Image image, float alpha)
    {
        Color tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;
    }

    private int FindChildIndex()
    {
        //현재 내가 몇번째 자식인지 확인
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i) == transform)
            {
                return i;
            }
        }
        return -1;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _currentItemIndex = FindChildIndex();
        transform.parent.GetComponent<DropZone_Old>().CurrentItemIndex = _currentItemIndex;
    }
}
