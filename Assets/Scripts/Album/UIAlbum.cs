using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum AlbumMenus
{
    Album,
    IllustratedGuide,

}

public class UIAlbum : MonoBehaviour
{
    [Tooltip("앨범 메뉴 스크립트 넣는 곳")]
    public UIAlbumAlbumMenu AlbumMenu;

    [Tooltip("클릭시 앨범 메뉴로 이동할 버튼을 넣는다")]
    [SerializeField] private Button _albumMenuButton;

    [SerializeField] private Button _illustratedGuide;

    public event Action OnActiveHandler;

    private void Awake()
    {
        _albumMenuButton.onClick.AddListener(() => ChangeActive(true));
    }

    private void Start()
    {
        ChangeActive(false);
    }

    public void ChangeActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {
            OnActiveHandler?.Invoke();
        }   
    }

    public void ChangeActive(bool isActived)
    {
        gameObject.SetActive(isActived);
        if(isActived)
        {
            OnActiveHandler?.Invoke();
        }      
    }

    public void SelectMenu(AlbumMenus option)
    {
        AlbumMenu.gameObject.SetActive(false);


        if (option == AlbumMenus.Album)
        {
            AlbumMenu.gameObject.SetActive(true);
        }
    }
}
