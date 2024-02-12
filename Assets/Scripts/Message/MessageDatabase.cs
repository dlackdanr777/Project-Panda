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
    public List<Message> MailList = new List<Message>();
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

        //Mail
        _dataMail = CSVReader.Read("MailList");

        for(int i=0;i<_dataMail.Count;i++)
        {
            MailList.Add(new Message(
                _dataMail[i]["ID"].ToString(),
                _dataMail[i]["NPC ID"].ToString(),
                _dataMail[i]["텍스트"].ToString(),
                GetItemById(_dataMail[i]["선물 ID"].ToString()),
                _dataMail[i]["스토리 단계"].ToString(),
                _dataMail[i]["게임 시간"].ToString(),
                GetItemSpriteById(_dataMail[i]["NPC ID"].ToString())));
        }
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
                return DatabaseManager.Instance.GetBugItemList()[GetIndexById(DatabaseManager.Instance.GetBugItemList(), id)];
            case "IFI":
                return DatabaseManager.Instance.GetFishItemList()[GetIndexById(DatabaseManager.Instance.GetFishItemList(), id)];
            case "IFR":
                return DatabaseManager.Instance.GetFruitItemList()[GetIndexById(DatabaseManager.Instance.GetFruitItemList(), id)];
            case "ITG":
                return DatabaseManager.Instance.GetGatheringToolItemList()[GetIndexById(DatabaseManager.Instance.GetGatheringToolItemList(), id)];
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
}
