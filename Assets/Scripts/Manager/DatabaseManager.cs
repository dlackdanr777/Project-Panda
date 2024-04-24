using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class DatabaseManager : SingletonHandler<DatabaseManager>
{
    private UserInfo _userInfo;
    public UserInfo UserInfo => _userInfo;

    private StarterPandaInfo _startPandaInfo;
    public StarterPandaInfo StartPandaInfo => _startPandaInfo;

    private PhotoDatabase _photoDatabase;
    public PhotoDatabase PhotoDatabase => _photoDatabase;

    private AttendanceDatabase _weatherDatabase;
    public AttendanceDatabase AttendanceDatabase => _weatherDatabase;

    private RecipeDatabase _recipeDatabase;
    public RecipeDatabase RecipeDatabase => _recipeDatabase;

    private MBTIManager _mbtiDatabase;

    private PandaManager _pandaDatabase;

    private ItemDatabase _itemDatabase;
    public ItemDatabase ItemDatabase => _itemDatabase;

    private FurniturePositionDatabase _furniturePosDatabase;
    public FurniturePositionDatabase FurniturePosDatabase => _furniturePosDatabase;

    private NPCDatabase _npcDatabase;
    public NPCDatabase NPCDatabase => _npcDatabase;

    private SideStoryDialogueManager _sideStoryDialogueDatabase;
    public SideStoryDialogueManager SideDialogueDatabase => _sideStoryDialogueDatabase;

    private MainStoryDialogueManager _mainStoryDialogueDatabase;
    public MainStoryDialogueManager MainDialogueDatabase => _mainStoryDialogueDatabase; 

    private ChallengesDatabase _challengesDatabase;
    public ChallengesDatabase ChallengesDatabase => _challengesDatabase;

    private Challenges _challenges;
    public Challenges Challenges => _challenges;

    private MessageDatabase _messageDatabase;
    public MessageDatabase MessageDatabase => _messageDatabase;

    private HarvestItemManager _harvestItemDatabase;
    public HarvestItemManager HarvestItemDatabase => _harvestItemDatabase;

    private NoticeDatabase _noticeDatabase;
    public NoticeDatabase NoticeDatabase => _noticeDatabase;


    //이미지들
    //Panda
    [Space]
    [SerializeField] private PandaImage _pandaImage;
    public PandaImage PandaImage => _pandaImage;

    [Space]
    //Item
    [SerializeField] private ItemSpriteDatabase[] _gatheringItemImages;
    public ItemSpriteDatabase[] GatheringItemImages => _gatheringItemImages;

    [SerializeField] private ItemSpriteDatabase _foodItemImages;
    public ItemSpriteDatabase FoodItemImages => _foodItemImages;

    [SerializeField] private ItemSpriteDatabase[] _toolItemImages;
    public ItemSpriteDatabase[] ToolItemImages => _toolItemImages;

    [SerializeField] private HarvestItemImage _harvestItemImage;
    public HarvestItemImage HarvestItemImage => _harvestItemImage;


    [Space]
    //NPC
    [SerializeField] private ItemSpriteDatabase[] _npcImages;
    public ItemSpriteDatabase[] NpcImages => _npcImages;

    [SerializeField] private ItemSpriteDatabase _npcIntimacyImages;

    [Space]
    //Furniture
    [SerializeField] private FurnitureSpriteDatabase _furnitureItemImages;
    public FurnitureSpriteDatabase FurnitureItemImages => _furnitureItemImages;

    [Space]
    //Message
    [SerializeField] private ItemSpriteDatabase _mailPaper;
    public ItemSpriteDatabase MailPaper => _mailPaper;

    [Space]
    //Album
    [SerializeField] private ItemSpriteDatabase _albumImages;
    public ItemSpriteDatabase AlbumImages => _albumImages;

    private AlbumDatabase _albumDatabase;
    public AlbumDatabase AlbumDatabase => _albumDatabase;

    [Space]
    //Sticker
    [SerializeField] private ItemSpriteDatabase _stickerImages;

    [Space]
    //Weather Image
    [SerializeField] private WeatherImage _weatherImages;
    public WeatherImage WeatherImage => _weatherImages;

    public override void Awake()
    {
        base.Awake();

        _userInfo = new UserInfo();
        _startPandaInfo = new StarterPandaInfo();
        _photoDatabase = new PhotoDatabase();
        _itemDatabase = new ItemDatabase();
        _weatherDatabase = new AttendanceDatabase();
        _recipeDatabase = new RecipeDatabase();
        _mbtiDatabase = new MBTIManager();
        _pandaDatabase = new PandaManager();
        _furniturePosDatabase = new FurniturePositionDatabase();
        _harvestItemDatabase = new HarvestItemManager();
        _npcDatabase = new NPCDatabase();
        _sideStoryDialogueDatabase = new SideStoryDialogueManager();
        _mainStoryDialogueDatabase = new MainStoryDialogueManager();
        _messageDatabase = new MessageDatabase();
        _albumDatabase = new AlbumDatabase();
        _challengesDatabase = new ChallengesDatabase();
        _challenges = new Challenges();
        _noticeDatabase = new NoticeDatabase();

        _itemDatabase.Register();
        _userInfo.Register();
        _startPandaInfo.Register();
        _photoDatabase.Register();
        _pandaDatabase.Register();
        _recipeDatabase.Register();
        _mbtiDatabase.Register();
        _furniturePosDatabase.Register();
        _harvestItemDatabase.Register();
        _weatherDatabase.Register();
        _npcDatabase.Register();
        _sideStoryDialogueDatabase.Register();
        _mainStoryDialogueDatabase.Register();
        _messageDatabase.Register();
        _albumDatabase.Register();
        _challengesDatabase.Register();
        _challenges.Register();
    }

    /// <summary>
    /// mbti�� ���� �Ǵ� ���� ����
    /// </summary>
    /// <param name="mbti"></param>
    /// <returns></returns>
    public Preference SetPreference(string mbti)
    {
        return _mbtiDatabase.SetPreference(mbti);
    }

    /// <summary>
    /// panda data
    /// </summary>
    /// <param name="pandaID">panda id</param>
    /// <returns></returns>
    public PandaData GetPandaData(int pandaID)
    {
        return _pandaDatabase.GetPandaData(pandaID);
    }

    /// <summary>
    /// panda image data
    /// </summary>
    /// <param name="pandaID">panda id</param>
    /// <returns></returns>
    public PandaStateImage GetPandaImage(int pandaID)
    {
        return _pandaDatabase.GetPandaImage(pandaID);
    }

    /// <summary>
    /// �Ǵ� ģ�е� ������Ʈ </summary>
    public void UpdatePandaIntimacy(int pandaID, float intimacy)
    {
        _pandaDatabase.UpdatePandaIntimacy(pandaID, intimacy);
    }

    /// <summary>
    /// �Ǵ� �ູ�� ������Ʈ </summary>
    public void UpdatePandaHappiness(int pandaID, float happiness)
    {
        _pandaDatabase.UpdatePandaHappiness(pandaID, happiness);
    }

    /// <summary>
    /// ��Ÿ�� �Ǵ� mbti ����</summary>
    public void SetStarterMBTI(string mbti)
    {
        _pandaDatabase.SetStarterMBTI(mbti);
    }


    public Item GetGIImageById(string id)
    {
        Dictionary<string, Item> allItemDic = _itemDatabase.AllItemDic;
        string code = id.Substring(0, 3);

        if(allItemDic.TryGetValue(id, out Item item))
        {
            return item;
        }

        Debug.Log("아이템이 존재하지 않습니다");
        return null;
    }

    /// <summary>
    /// NPC List
    /// </summary>
    public List<NPC> GetNPCList()
    {
        return _npcDatabase.NpcList;
    }

    /// <summary>
    /// NPC ID로 이름 찾기
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetNPCNameById(string id)
    {
        for (int i = 0; i < GetNPCList().Count; i++)
        {
            if (GetNPCList()[i].Id.Equals(id))
            {
                return GetNPCList()[i].Name;
            }
        }
        return null;
    }

    /// <summary>
    /// NPC ID로 Image 찾기
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Sprite GetNPCImageById(string id)
    {
        for (int i = 0; i < GetNPCList().Count; i++)
        {
            if (GetNPCList()[i].Id.Equals(id))
            {
                return GetNPCList()[i].Image;
            }
        }
        return null;
    }

    public Sprite GetNPCIntimacyImageById(string id)
    {
        for (int i = 0; i < GetNPCList().Count; i++)
        {
            if (GetNPCList()[i].Id.Equals(id))
            {
                NPC currentNPC = GetNPCList()[i];
                string intimacy = GetIntimacy(currentNPC.Intimacy, currentNPC.MinIntimacy, currentNPC.MaxIntimacy).ToString();

                for (int j = 0; j < _npcIntimacyImages.ItemSprites.Length; j++)
                {
                    if (_npcIntimacyImages.ItemSprites[j].Id.Equals(intimacy))
                    {
                        return _npcIntimacyImages.ItemSprites[j].Image;
                    }
                }
            }
        }
        return null;
    }

    private int GetIntimacy(int amount, int minIntimacy, int maxIntimacy)
    {
        amount /= 20;

        if (amount < minIntimacy)
        {
            return minIntimacy;
        }
        else if (amount >= maxIntimacy)
        {
            return maxIntimacy;
        }

        return amount;
    }

    public NPC GetNPC(string id)
    {
        for (int i = 0; i < GetNPCList().Count; i++)
        {
            if (GetNPCList()[i].Id.Equals(id))
            {
                return GetNPCList()[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 친밀도 증가 함수
    /// </summary>
    /// <param name="id">NPCID</param>
    /// <param name="count">증가할 양</param>
    public void AddIntimacy(string id, int count)
    {
        for (int i = 0; i < _npcDatabase.NpcList.Count; i++)
        {
            if (_npcDatabase.NpcList[i].Id.Equals(id))
            {
                _npcDatabase.NpcList[i].Intimacy += count;
            }
        }
    }

    /// <summary>
    /// Furniture ItemList
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, Furniture> GetFurnitureItem()
    {
        return _itemDatabase.FurnitureDic;
    }

    /// <summary>
    /// FurnitureType List
    /// </summary>
    /// <returns></returns>
    public FurnitureType[] GetFurnitureTypeList()
    {
        return _itemDatabase.FurnitureTypeList;
    }

    /// <summary>
    /// Mail List
    /// </summary>
    public List<Message> GetMailList()
    {
        return _messageDatabase.MailList;
    }




    /// <summary>
    /// 기존의 스티커 이미지 출력
    /// </summary>
    /// <returns></returns>
    public ItemSpriteDatabase GetStickerImages()
    {
        return _stickerImages;
    }

    public Sprite GetStickerImage(string id)
    {
        for(int i = 0, count = _stickerImages.ItemSprites.Length; i < count; i++)
        {
            if (_stickerImages.ItemSprites[i].Id == id)
                return _stickerImages.ItemSprites[i].Image;
        }

        Debug.LogFormat("{0} 해당 이미지가 존재하지 않습니다.", id);
        return null;
    }


    public HarvestItem GetHarvestItemdata(string harvestItemID)
    {
        return _harvestItemDatabase.GetHarvestItemdata(harvestItemID);
    }

    public Dictionary<string, ChallengesData> GetChallengesDic()
    {
        return _challengesDatabase.GetChallengesDic();
    }
}
