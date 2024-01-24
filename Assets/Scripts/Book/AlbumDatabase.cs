using BackEnd;
using LitJson;
using Muks.BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbumDatabase : MonoBehaviour
{
    //Mail
    public List<Album> AlbumList = new List<Album>();

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

        //임시로 리소스 폴더에 있는걸 쓰는중
        //정식 출시전엔 뺄 예정
        AlbumParserByLocal();
    }

    public void LoadData()
    {
        BackendManager.Instance.GetChartData("105930", 10, AlbumParserByServer);
    }


    /// <summary>리소스 폴더에서 앨범 정보를 받아와 리스트에 넣는 함수</summary>
    private void AlbumParserByLocal()
    {
        List<Dictionary<string, object>> _dataAlbum = CSVReader.Read("Album");

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


    /// <summary>서버에서 앨범 정보를 받아와 리스트에 넣는 함수</summary>
    private void AlbumParserByServer(BackendReturnObject callback)
    {
        AlbumList.Clear();
        JsonData json = callback.FlattenRows();

        for (int i = 0, count = json.Count; i < count; i++)
        {
            string albumID = json[i]["AlbumID"].ToString();
            string name = json[i]["Name"].ToString();
            string description = json[i]["Description"].ToString();
            string storyStep = json[i]["StoryStep"].ToString();
            Sprite sprite = GetItemSpriteById(albumID);
            AlbumList.Add(new Album(albumID, name, description, storyStep, sprite));
        }

        Debug.Log("앨범 정보 받아오기 성공");
    }

    private Sprite GetItemSpriteById(string id)
    {
        Sprite sprite = _albumSpriteDic[id];
        return sprite;
    }
}
