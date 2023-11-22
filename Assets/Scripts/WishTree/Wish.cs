using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wish : MonoBehaviour
{
    private Button _wishButton;

    private void Start()
    {
        _wishButton = GetComponent<Button>();

        _wishButton.onClick.AddListener(OnClickWishButton);
    }

    private void OnClickWishButton()
    {
        //�� �ҿ� ui setactive true
        Destroy(gameObject);
    }
}
