using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbumDatabase : MonoBehaviour
{
    public Album[] Albums;

    //Mail
    public List<Album> AlbumList = new List<Album>();
    private List<Dictionary<string, object>> _dataAlbum;

    //MailPaper
    public ItemSpriteDatabase AlbumSpriteArray;
    public Dictionary<string, Sprite> _albumSpriteDic;

    public void Register()
    {
        //편지지 Image
        _albumSpriteDic = new Dictionary<string, Sprite>();
        for (int i = 0; i < AlbumSpriteArray.ItemSprites.Length; i++)
        {
            _albumSpriteDic.Add(AlbumSpriteArray.ItemSprites[i].Id, AlbumSpriteArray.ItemSprites[i].Image);
        }

        //Mail
        _dataAlbum = CSVReader.Read("Album");

        for (int i = 0; i < _dataAlbum.Count; i++)
        {
            AlbumList.Add(new Album(
                _dataAlbum[i]["ID"].ToString(),
                _dataAlbum[i]["이름"].ToString(),
                _dataAlbum[i]["설명"].ToString(),
                _dataAlbum[i]["스토리 단계"].ToString(),
                GetItemSpriteById(_dataAlbum[i]["ID"].ToString())));
        }
        DatabaseManager.Instance.UserInfo.LoadUserReceivedAlbum();
    }

    private Sprite GetItemSpriteById(string id)
    {
        Sprite sprite = _albumSpriteDic[id];
        return sprite;
    }
}
