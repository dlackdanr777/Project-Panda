using BackEnd;
using LitJson;
using Muks.BackEnd;
using System.Collections.Generic;
using UnityEngine;


/// <summary> ������ ���� ������ �����ϴ� Ŭ���� </summary>
public class BookUserData
{
    //Sticker
    public List<string> AlbumReceived = new List<string>();
    public List<string> StickerReceived = new List<string>();
    public List<ServerStickerData> StickerDataList = new List<ServerStickerData>();

    //���ʹϾ� ���� ������ �ø� �� �����Ƿ� �߰��� ������ �� Class List
    private List<SaveStickerData> _saveStickerDataList = new List<SaveStickerData>();


    public List<ServerStickerData> GetStickerList()
    {
        return StickerDataList;
    }


    #region Save&Load Book

    /// <summary> ���������� ���� ���� ���� ������ �ҷ��� </summary>
    public void LoadBookData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
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
            Debug.Log("Book Load����");
        }
    }

    /// <summary> ���������� ���� ���� ���� ���� ���� </summary>
    public void SaveBookData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Book";

        if (!Backend.IsLogin)
        {
            Debug.LogError("�ڳ��� �α��� �Ǿ����� �ʽ��ϴ�.");
            return;
        }

        if (maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} ��Ʈ�� ������ �޾ƿ��� ���߽��ϴ�.", selectedProbabilityFileId);
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

                Debug.LogFormat("{0} ������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary> �񵿱������� ���� ���� ���� ���� ���� </summary>
    public void AsyncSaveBookData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Book";

        if (!Backend.IsLogin)
        {
            Debug.LogError("�ڳ��� �α��� �Ǿ����� �ʽ��ϴ�.");
            return;
        }

        if (maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} ��Ʈ�� ������ �޾ƿ��� ���߽��ϴ�.", selectedProbabilityFileId);
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

                    Debug.LogFormat("{0} ������ �����߽��ϴ�..", selectedProbabilityFileId);
                    break;
            }
        }); 
    }


    /// <summary> ���������� ���� ���� ���� ���� ���� </summary>
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

        Debug.LogFormat("{0} ������ ������ ��û�մϴ�.", selectedProbabilityFileId);

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> ���������� ���� ���� ���� ���� ���� </summary>
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

        Debug.LogFormat("{0} ������ ������ ��û�մϴ�.", selectedProbabilityFileId);

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> �񵿱������� ���� ���� ���� ���� ���� </summary>
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

        Debug.LogFormat("{0} ������ ������ ��û�մϴ�.", selectedProbabilityFileId);

        BackendManager.Instance.AsyncGameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> �񵿱������� ���� ���� ���� ���� ���� </summary>
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

        Debug.LogFormat("{0} ������ ������ ��û�մϴ�.", selectedProbabilityFileId);

        BackendManager.Instance.AsyncGameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ���� �����͸� ��� ��ȯ�ϴ� Ŭ���� </summary>
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

        Debug.LogErrorFormat("{0}Id�� �������� �ʽ��ϴ�.");
        return null;
    }

    #endregion
}
