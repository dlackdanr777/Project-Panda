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

public class UIAlbum : UIView
{
    [Tooltip("�ٹ� �޴� ��ũ��Ʈ �ִ� ��")]
    public UIAlbumAlbumMenu AlbumMenu;

    [Tooltip("Ŭ���� �ٹ� �޴��� �̵��� ��ư�� �ִ´�")]
    [SerializeField] private Button _albumMenuButton;

    [SerializeField] private Button _illustratedGuide;

    public event Action OnActiveHandler;

    public void Awake()
    {
        gameObject.SetActive(false);
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
