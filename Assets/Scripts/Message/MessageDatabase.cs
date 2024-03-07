using System.Collections.Generic;
using UnityEngine;

public enum MessageField
{
    None = -1,
    Mail,
    Wish
}


public class MessageDatabase
{
    public Message[] Messages;

    //Mail
    private List<Message> _mailList = new List<Message>();
    public List<Message> MailList => _mailList;
    private List<Dictionary<string, object>> _dataMail;

    //MailPaper
    public Dictionary<string, Sprite> _mailPaperSpriteDic;

    public void Register()
    {
        //편지지 Image
        _mailPaperSpriteDic = new Dictionary<string, Sprite>();

        ItemSpriteDatabase mailPaper = DatabaseManager.Instance.MailPaper;

        for (int i=0; i < mailPaper.ItemSprites.Length; i++)
        {
            _mailPaperSpriteDic.Add(mailPaper.ItemSprites[i].Id, mailPaper.ItemSprites[i].Image);
        }

        MessageParseByLocal();
    }

    private Sprite GetItemSpriteById(string id)
    {
        id = GetPaperId(id);
        Sprite sprite = _mailPaperSpriteDic[id];
        return sprite;
    }

    private string GetPaperId(string npcId)
    {
        for(int i=0;i<DatabaseManager.Instance.NPCDatabase.NpcList.Count; i++)
        {
            if (npcId.Equals(DatabaseManager.Instance.NPCDatabase.NpcList[i].Id))
            {
                return DatabaseManager.Instance.NPCDatabase.NpcList[i].MessagePaper;
            }
        }
        return null;
    }

    private Item GetItemById(string id)
    {
        string startId = id.Substring(0, 3); //앞 3글자로 아이디 비교
        switch (startId)
        {
            case "IBG":
                return DatabaseManager.Instance.ItemDatabase.ItemBugList[GetIndexById(DatabaseManager.Instance.ItemDatabase.ItemBugList, id)];
            case "IFI":
                return DatabaseManager.Instance.ItemDatabase.ItemFishList[GetIndexById(DatabaseManager.Instance.ItemDatabase.ItemFishList, id)];
            case "IFR":
                return DatabaseManager.Instance.ItemDatabase.ItemFruitList[GetIndexById(DatabaseManager.Instance.ItemDatabase.ItemFruitList, id)];
            case "ITG":
                return DatabaseManager.Instance.ItemDatabase.ItemToolList[GetIndexById(DatabaseManager.Instance.ItemDatabase.ItemToolList, id)];
            default:
                return null;
        }
    }

    private int GetIndexById(List<GatheringItem> itemList, string id)
    {
        for(int i=0;i<itemList.Count ; i++)
        {
            if (itemList[i].Id.Equals(id))
            {
                return i;
            }
        }
        return -1;
    }

    private int GetIndexById(List<ToolItem> itemList, string id)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].Id.Equals(id))
            {
                return i;
            }
        }
        return -1;
    }


    /// <summary>메일리스트 csv파일을 인 게임에서 사용할 수 있게 List에 저장하는 함수</summary>
    private void MessageParseByLocal()
    {
        _dataMail = CSVReader.Read("MailList");

        for (int i = 0; i < _dataMail.Count; i++)
        {
            string id = _dataMail[i]["Id"].ToString();
            string ncpId = _dataMail[i]["NpcId"].ToString();
            string text = _dataMail[i]["Context"].ToString();
            Item gift = GetItemById(_dataMail[i]["GiftItemId"].ToString());
            string storyStep = _dataMail[i]["StoryStep"].ToString();
            string time = _dataMail[i]["GameTime"].ToString();
            Sprite sprite = GetItemSpriteById(ncpId);
            Message message = new Message(id, ncpId, text, gift, storyStep, time, sprite);
            _mailList.Add(message);
        }
    }
}
