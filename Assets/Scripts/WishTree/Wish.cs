using Muks.DataBind;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Wish : MonoBehaviour
{
    private Button _wishButton;
    private GameObject _detailView;

    public Message Message;

    private void Start()
    {
        _wishButton = GetComponent<Button>();
        _detailView = transform.parent.parent.GetChild(2).gameObject;

        _wishButton.onClick.AddListener(OnClickWishButton);
    }

    private void OnClickWishButton()
    {
        DataBind.SetTextValue("WishDetailTo", Message.To);
        DataBind.SetTextValue("WishDetailContent", Message.Content);
        DataBind.SetTextValue("WishDetailFrom", Message.From);
        _detailView.SetActive(true);
        //상세 소원 ui setactive true
        Destroy(gameObject);
    }
}
