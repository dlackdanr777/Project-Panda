using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDropZone2D : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject _itemPf;
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray = new Ray2D(touchPosition, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            Debug.Log(hit.collider.name);
        if(hit.collider == null)
        {
            Instantiate(_itemPf, transform);
            _itemPf.GetComponent<Image>().sprite = hit.collider.gameObject.GetComponent<Image>().sprite;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
