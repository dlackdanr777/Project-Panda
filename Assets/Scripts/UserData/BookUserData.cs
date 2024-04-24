using BackEnd;
using LitJson;
using Muks.BackEnd;
using System.Collections.Generic;
using UnityEngine;


/// <summary> 유저의 도감 정보를 보관하는 클래스 </summary>
public class BookUserData
{
    //Sticker
    public List<string> AlbumReceived = new List<string>();
    public List<string> StickerReceived = new List<string>();
    public List<ServerStickerData> StickerDataList = new List<ServerStickerData>();

    //쿼터니언 값을 서버에 올릴 수 없으므로 중간에 관리해 줄 Class List
    private List<SaveStickerData> _saveStickerDataList = new List<SaveStickerData>();


    public List<ServerStickerData> GetStickerList()
    {
        return StickerDataList;
    }


    #region Save&Load Book

    /// <summary> 동기적으로 서버 유저 도감 정보를 불러옴 </summary>
    public void LoadBookData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("데이터가 존재하지 않습니다.");
            return;
        }

        else
        {
            for (int i = 0, count = json[0]["AlbumReceived"].Count; i < count; i++)
            {
                string item = json[0]["AlbumReceived"][i].ToString();
                AlbumReceived.Add(item);
            }

            for (int i = 0, count = json[0]["StickerReceived"].Count; i < count; i++)
            {
                string item = json[0]["StickerReceived"][i].ToString();
                StickerReceived.Add(item);
            }

            _saveStickerDataList.Clear();
            for (int i = 0; i < json[0]["StickerDataArray"].Count; i++)
            {
                SaveStickerData item = JsonUtility.FromJson<SaveStickerData>(json[0]["StickerDataArray"][i].ToJson());
                _saveStickerDataList.Add(item);
                StickerDataList.Add(item.GetStickerData());
            }

            LoadAlbumReceived();
            LoadStickerReceived();
            Debug.Log("Book Load성공");
        }
    }

    /// <summary> 동기적으로 서버 유저 도감 정보 저장 </summary>
    public void SaveBookData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Book";

        if (!Backend.IsLogin)
        {
            Debug.LogError("뒤끝에 로그인 되어있지 않습니다.");
            return;
        }

        if (maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} 차트의 정보를 받아오지 못했습니다.", selectedProbabilityFileId);
            return;
        }

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                break;

            case BackendState.Maintainance:
                break;

            case BackendState.Retry:
                SaveBookData(maxRepeatCount - 1);
                break;

            case BackendState.Success:

                if (bro.GetReturnValuetoJSON() != null)
                {
                    if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                    {
                        InsertBookData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdateBookData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertBookData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0} 정보를 저장했습니다..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary> 비동기적으로 서버 유저 도감 정보 저장 </summary>
    public void AsyncSaveBookData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Book";

        if (!Backend.IsLogin)
        {
            Debug.LogError("뒤끝에 로그인 되어있지 않습니다.");
            return;
        }

        if (maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} 차트의 정보를 받아오지 못했습니다.", selectedProbabilityFileId);
            return;
        }

        Backend.GameData.Get(selectedProbabilityFileId, new Where(), bro =>
        {
            switch (BackendManager.Instance.ErrorCheck(bro))
            {
                case BackendState.Failure:
                    break;

                case BackendState.Maintainance:
                    break;

                case BackendState.Retry:
                    AsyncSaveBookData(maxRepeatCount - 1);
                    break;

                case BackendState.Success:

                    if (bro.GetReturnValuetoJSON() != null)
                    {
                        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                        {
                            AsyncInsertBookData(selectedProbabilityFileId);
                        }
                        else
                        {
                            AsyncUpdateBookData(selectedProbabilityFileId, bro.GetInDate());
                        }
                    }
                    else
                    {
                        AsyncInsertBookData(selectedProbabilityFileId);
                    }

                    Debug.LogFormat("{0} 정보를 저장했습니다..", selectedProbabilityFileId);
                    break;
            }
        }); 
    }


    /// <summary> 동기적으로 서버 유저 도감 정보 삽입 </summary>
    public void InsertBookData(string selectedProbabilityFileId)
    {
        SaveAlbumReceived();
        SaveStickerReceived();

        _saveStickerDataList.Clear();
        foreach (ServerStickerData data in StickerDataList)
        {
            _saveStickerDataList.Add(data.GetSaveStickerData());
        }

        Param param = GetBookParam();

        Debug.LogFormat("{0} 데이터 삽입을 요청합니다.", selectedProbabilityFileId);

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> 동기적으로 서버 유저 도감 정보 수정 </summary>
    public void UpdateBookData(string selectedProbabilityFileId, string inDate)
    {
        SaveAlbumReceived();
        SaveStickerReceived();

        _saveStickerDataList.Clear();
        foreach (ServerStickerData data in StickerDataList)
        {
            _saveStickerDataList.Add(data.GetSaveStickerData());
        }

        Param param = GetBookParam();

        Debug.LogFormat("{0} 데이터 수정을 요청합니다.", selectedProbabilityFileId);

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 비동기적으로 서버 유저 도감 정보 삽입 </summary>
    public void AsyncInsertBookData(string selectedProbabilityFileId)
    {
        SaveAlbumReceived();
        SaveStickerReceived();

        _saveStickerDataList.Clear();
        foreach (ServerStickerData data in StickerDataList)
        {
            _saveStickerDataList.Add(data.GetSaveStickerData());
        }

        Param param = GetBookParam();

        Debug.LogFormat("{0} 데이터 삽입을 요청합니다.", selectedProbabilityFileId);

        BackendManager.Instance.AsyncGameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> 비동기적으로 서버 유저 도감 정보 수정 </summary>
    public void AsyncUpdateBookData(string selectedProbabilityFileId, string inDate)
    {
        SaveAlbumReceived();
        SaveStickerReceived();

        _saveStickerDataList.Clear();
        foreach (ServerStickerData data in StickerDataList)
        {
            _saveStickerDataList.Add(data.GetSaveStickerData());
        }

        Param param = GetBookParam();

        Debug.LogFormat("{0} 데이터 수정을 요청합니다.", selectedProbabilityFileId);

        BackendManager.Instance.AsyncGameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 서버에 도감 데이터를 모아 반환하는 클래스 </summary>
    public Param GetBookParam()
    {
        Param param = new Param();

        param.Add("AlbumReceived", AlbumReceived);
        param.Add("StickerReceived", StickerReceived);
        param.Add("StickerDataArray", _saveStickerDataList);

        return param;
    }


    private void LoadAlbumReceived()
    {
        List<Album> albumList = DatabaseManager.Instance.AlbumDatabase.GetAlbumList();

        for (int i = 0; i < AlbumReceived.Count; i++)
        {
            Album album = albumList.Find(x => x.Id == AlbumReceived[i]);
            if (album == null)
                continue;

            album.IsReceived = true;
        }
    }


    private void SaveAlbumReceived()
    {
        List<Album> albumList = DatabaseManager.Instance.AlbumDatabase.GetAlbumList();

        for (int i = 0; i < albumList.Count; i++)
        {
            if (AlbumReceived.Find((x) => x == albumList[i].Id) != null)
                continue;

            AlbumReceived.Add(albumList[i].Id);
        }
    }


    private void SaveStickerReceived()
    {
        StickerReceived.Clear();
        for (int i = 0; i < GameManager.Instance.Player.StickerInventory.Count; i++)
        {
            StickerReceived.Add(GameManager.Instance.Player.StickerInventory.GetStickerList()[i].Id);
        }
    }


    private void LoadStickerReceived()
    {
        // Sticker
        for (int i = 0; i < StickerReceived.Count; i++)
        {
            GameManager.Instance.Player.StickerInventory.AddById(StickerReceived[i], GetStickerImage(StickerReceived[i]));
        }
    }

    private Sprite GetStickerImage(string id)
    {
        string code = id.Substring(0, 3);
        Sprite image = FindItemSpriteById(id);

        switch (code)
        {
            case "NPC":
                image = FindNPCSpriteById(id);
                break;

            default:
                image = FindItemSpriteById(id);
                break;
        }

        return image;
    }


    private Sprite FindItemSpriteById(string id)
    {
        return DatabaseManager.Instance.GetStickerImage(id);
    }


    private Sprite FindNPCSpriteById(string id)
    {
        Dictionary<string, NPC> npcDic = DatabaseManager.Instance.NPCDatabase.NpcDic;
        if (npcDic.TryGetValue(id, out NPC npc))
        {
            return npc.Image;
        }

        Debug.LogErrorFormat("{0}Id가 존재하지 않습니다.");
        return null;
    }

    #endregion
}
