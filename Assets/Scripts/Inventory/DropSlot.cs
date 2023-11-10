using Muks.DataBind;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _selectedItem;
    [SerializeField] private GameObject _dropPopup;
    //[SerializeField] private ClickPhone _clickPhone;

    private Transform _spawnPoint;
    public event Action OnUseItem;

    private void Start()
    {
        DataBind.SetButtonValue("ItemDropPopupCloseBtn", OnClickedNoItemDrop);
        DataBind.SetButtonValue("ItemDropBtn", OnClickedItemDrop);

        //_clickPhone.OnRemoveSelectedItem += _clickPhone_OnRemoveSelectedItem;
    }

    private void _clickPhone_OnRemoveSelectedItem()
    {
        RemoveSelectedItem();
    }

    private void SpawnImage(bool isSpawn) //spawn�ϰų� ���� �� ��
    {
        Image spawnImage = _spawnPoint.transform.GetChild(0).GetComponent<Image>();
        Color tempColor = spawnImage.color;

        if (isSpawn)
        {
            spawnImage.gameObject.transform.position = _spawnPoint.position;
            //image setactive
            tempColor.a = 255f;
            spawnImage.color = tempColor;

            _spawnPoint.transform.GetChild(0).GetComponent<Image>().sprite = _selectedItem.GetComponent<SpriteRenderer>().sprite;

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
        _dropPopup.SetActive(false);
        RemoveSelectedItem();
        OnUseItem?.Invoke();
        
    }
    private void OnClickedNoItemDrop()
    {
        SpawnImage(false);
        _dropPopup.SetActive(false);
        _selectedItem.SetActive(true);
    }

    public void RemoveSelectedItem()//����ٴϴ� ��ü ����
    {
        _selectedItem.GetComponent<SpriteRenderer>().sprite = null;
        _selectedItem.SetActive(false);
    }

    //slot�� ������ ��
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        _spawnPoint = eventData.pointerCurrentRaycast.gameObject.transform; //���� ����
        if(_spawnPoint != null)
        {
            //�ȿ� �̹����� ������(���õ� �������� �ִٸ�), �Ҵ�� �������� ���ٸ�
            if (_selectedItem.GetComponent<SpriteRenderer>().sprite != null && _spawnPoint.transform.GetChild(0).GetComponent<Image>().sprite == null)
            {
                //��ġ�� ������ popup
                _dropPopup.gameObject.SetActive(true);
                SpawnImage(true); //�ش� ��ġ�� spawn
                _selectedItem.SetActive(false); //����ٴϴ� ������ ������ �ʵ���
            }

        }
    }
}
