using BackEnd;
using LitJson;
using Muks.BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbumDatabase : MonoBehaviour
{
    private List<Album> _albumList = new List<Album>();
    private int Count => _albumList.Count;

    private Dictionary<string, Sprite> _albumSpriteDic;

    public void Register()
    {
        //앨범 Image
        _albumSpriteDic = new Dictionary<string, Sprite>();

        ItemSpriteDatabase albumSprites = DatabaseManager.Instance.AlbumImages;
        for (int i = 0; i < albumSprites.ItemSprites.Length; i++)
        {
            _albumSpriteDic.Add(albumSprites.ItemSprites[i].Id, albumSprites.ItemSprites[i].Image);
        }

        //임시로 리소스 폴더에 있는걸 쓰는중
        //정식 출시전엔 뺄 예정
        AlbumParserByLocal();
    }

    public void LoadData()
    {
        BackendManager.Instance.GetChartData("105930", 10, AlbumParserByServer);
    }


    public List<Album> GetAlbumList()
    {
        return _albumList;
    }


    public void SetReceiveAlbumById(string id)
    {
        for (int i = 0, count = _albumList.Count; i < count; i++)
        {
            if (_albumList[i].StoryStep.Equals(id))
            {
                _albumList[i].IsReceived = true;
                return;
            }
        }
    }


    /// <summary>리소스 폴더에서 앨범 정보를 받아와 리스트에 넣는 함수</summary>
    private void AlbumParserByLocal()
    {
        List<Dictionary<string, object>> _dataAlbum = CSVReader.Read("Album");

        for (int i = 0; i < _dataAlbum.Count; i++)
        {
            _albumList.Add(new Album(
                _dataAlbum[i]["ID"].ToString(),
                _dataAlbum[i]["이름"].ToString(),
                _dataAlbum[i]["설명"].ToString(),
                _dataAlbum[i]["스토리 단계"].ToString(),
                GetItemSpriteById(_dataAlbum[i]["ID"].ToString())));
        }
    }


    /// <summary>서버에서 앨범 정보를 받아와 리스트에 넣는 함수</summary>
    private void AlbumParserByServer(BackendReturnObject callback)
    {
        _albumList.Clear();
        JsonData json = callback.FlattenRows();

        for (int i = 0, count = json.Count; i < count; i++)
        {
            string albumID = json[i]["AlbumID"].ToString();
            string name = json[i]["Name"].ToString();
            string description = json[i]["Description"].ToString();
            string storyStep = json[i]["StoryStep"].ToString();
            Sprite sprite = GetItemSpriteById(albumID);
            _albumList.Add(new Album(albumID, name, description, storyStep, sprite));
        }

        Debug.Log("앨범 정보 받아오기 성공");
    }

    private Sprite GetItemSpriteById(string id)
    {
        Sprite sprite = _albumSpriteDic[id];
        return sprite;
    }
}
