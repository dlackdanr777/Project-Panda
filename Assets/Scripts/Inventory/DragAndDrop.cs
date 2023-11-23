using Muks.Tween;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler //arrange button�� ���
{
    //�̹����� ��������
    [SerializeField] private GameObject _selectedItem; //������ ������
    [SerializeField] private GameObject _dropPopup;
    [SerializeField] private GameObject _moveObject;
    [SerializeField] private GameObject _itemDropZone;
    [SerializeField] private Transform _itemZone;
    [SerializeField] private Button _itemDropButton;
    [SerializeField] private Button _itemNoDropButton;

    private float speed = 20f;

    public event Action OnUseItem;
    public event Action DontUseItem;

    private void Start()
    {
        _itemDropButton.onClick.AddListener(OnClickedItemDrop);
        _itemNoDropButton.onClick.AddListener(OnClickedNoItemDrop);
    }
    private void SpawnImage(bool isSpawn) //slot�� spawn�ϰų� ���� �� ��
    {
        Image spawnImage = _itemZone.GetComponent<Image>();
        Color tempColor = spawnImage.color;

        if (isSpawn)
        {
            spawnImage.gameObject.transform.position = _itemZone.position;
            //image setactive
            tempColor.a = 255f;
            spawnImage.color = tempColor;

            _itemZone.GetComponent<Image>().sprite = _selectedItem.GetComponent<SpriteRenderer>().sprite;

        }
        else
        {
            tempColor.a = 0f;
            spawnImage.color = tempColor;

            spawnImage.sprite = null;
        }
    }

    private void OnClickedItemDrop()
    {
        _dropPopup.SetActive(false); //popup �����
        _itemDropZone.gameObject.SetActive(false); //map item zone �����
        MoveObject(1); //�� �ö�
        OnUseItem?.Invoke(); //item ��� detailview �����
        RemoveSelectedItem(); //selected image �����
        _moveObject.SetActive(false); //�κ��丮 �����
    }
    private void OnClickedNoItemDrop()
    {
        SpawnImage(false); //slot�� ������ ������
        _dropPopup.SetActive(false);//popup �����
        _itemDropZone.gameObject.SetActive(false); //map item zone �����
        MoveObject(1); //�� �ö�
        DontUseItem?.Invoke(); //detailview �����
        _selectedItem.SetActive(false);
        _moveObject.SetActive(false); //�κ��丮 �����
    }

    private void MoveObject(int gap)
    {
        //_moveObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(!_moveObject.transform.GetChild(0).GetChild(0).gameObject.activeSelf); //�� border button set active false
        Tween.TransformMove(_moveObject, new Vector3(_moveObject.transform.position.x,  gap, _moveObject.transform.position.z), 1f, TweenMode.Smoothstep);
    }

    private void RemoveSelectedItem()//����ٴϴ� ��ü ����
    {
        _selectedItem.GetComponent<SpriteRenderer>().sprite = null;
        _selectedItem.SetActive(false);
    }
    public void OnBeginDrag(PointerEventData eventData) //drag �����ϸ� ������ ������
    {
        Vector3 pos = eventData.position;
        pos.z = 10;
        _selectedItem.transform.position = Camera.main.ScreenToWorldPoint(pos);
        _selectedItem.SetActive(true);

        MoveObject(-80);

        _itemDropZone.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData) //�������� �ش� ��ġ�� ���� ������
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);

        mousePosition = new Vector2(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y));
        _selectedItem.transform.position = Vector2.Lerp(_selectedItem.transform.position, mousePosition, Time.deltaTime * speed);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        try
        {
            _itemZone = eventData.pointerCurrentRaycast.gameObject.transform; //���� ����
            if (_itemZone != null)
            {
                //�Ҵ�� �������� ���ٸ�
                if ( _itemZone.GetComponent<Image>().sprite == null)
                {
                    //��ġ�� ������ popup
                    _dropPopup.gameObject.SetActive(true);
                    SpawnImage(true); //�ش� ��ġ�� spawn
                }
                else //�Ҵ�� �������� �ִٸ�, map�� �ƴ� ���� 
                {
                    _selectedItem.GetComponent<SpriteRenderer>().sprite = null;
                    MoveObject(1);
                    _itemDropZone.SetActive(false);
                }
            }
        }
        catch (Exception) //����� ��濡 �ƹ��͵� ��� ���� ó���س���,, ���߿� map �򸮸� ����
        {
            _selectedItem.GetComponent<SpriteRenderer>().sprite = null;
            MoveObject(1);
            _itemDropZone.SetActive(false);
        }

        _selectedItem.SetActive(false); //����ٴϴ� ������ ������ �ʵ���

        

    }
}
