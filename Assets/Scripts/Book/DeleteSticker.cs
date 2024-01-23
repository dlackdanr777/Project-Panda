using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteSticker : MonoBehaviour
{
    [SerializeField] private GameObject _sticker;
    private Button _delete;

    private void Start()
    {
        _delete = GetComponent<Button>();
        _delete.onClick.AddListener(OnClickDeleteButton);
    }

    private void OnClickDeleteButton()
    {
        Destroy(_sticker);
    }
}
