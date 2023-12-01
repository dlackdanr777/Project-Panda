using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {

        //if () //ÇöÀç 
        {

            foreach (Image item in transform.GetComponentsInChildren<Image>())
            {
                if (item.sprite == null)
                {
                    ChangeAlpha(item, 0.6f);
                    
                }

            }

        }
    } 

    public void OnPointerExit(PointerEventData eventData)
    {
        //if (this.GetComponent<ItemSlot>() != null)
        {
            foreach (Image item in transform.GetComponentsInChildren<Image>())
            {
                if (item.sprite == null)
                {
                    ChangeAlpha(item, 0f);

                }

            }
        }
    }

    private void ChangeAlpha(Image image, float alpha)
    {
        Color tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;
    }

}
