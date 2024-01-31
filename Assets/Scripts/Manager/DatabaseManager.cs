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

    private DialogueManager _dialogueDatabase;
    public DialogueManager DialogueDatabase => _dialogueDatabase;
 
    private WeatherApp _weatherDatabase;
    public WeatherApp WeatherDatabase => _weatherDatabase;

    private RecipeDatabase _recipeDatabase;
    public RecipeDatabase RecipeDatabase => _recipeDatabase;

    private MBTIManager _mbtiDatabase;

    [SerializeField] private PandaImage _pandaImage;
    private PandaManager _pandaDatabase;

    //Item
    [SerializeField] private ItemSpriteDatabase[] _gatheringItemImages;
    [SerializeField] private ItemSpriteDatabase[] _toolItemImages;
    [SerializeField] private ItemSpriteDatabase _furnitureItemImages;
    [SerializeField] private ItemSpriteDatabase[] _npcImages;

    private ItemDatabase _itemDatabase;
    public ItemDatabase ItemDatabase => _itemDatabase;
    
    private FurniturePositionDatabase _furniturePosDatabase;
    public FurniturePositionDatabase FurniturePosDatabase => _furniturePosDatabase;

    private HarvestItemManager _harvestItemDatabase;
    [SerializeField] HarvestItemImage _harvestItemImage;

    private NPCDatabase _npcDatabase;
    public NPCDatabase NPCDatabase => _npcDatabase;

    private SideStoryDialogueManager _sideStoryDialogueDatabase;
    public SideStoryDialogueManager SideDialogueDatabase => _sideStoryDialogueDatabase;

    //Message
    [SerializeField] private ItemSpriteDatabase _mailPaper;
    private MessageDatabase _messageDatabase;
    public MessageDatabase MessageDatabase => _messageDatabase;

    //Album
    [SerializeField] private ItemSpriteDatabase _albumImages;
    private AlbumDatabase _albumDatabase;
    public AlbumDatabase AlbumDatabase => _albumDatabase;

    //Sticker
    [SerializeField] private ItemSpriteDatabase _stickerImages;

    private ChallengesDatabase _challengesDatabase;
    private Challenges _challenges;
    public Challenges Challenges => _challenges;

    public override void Awake()
    {
        var obj = FindObjectsOfType<DatabaseManager>();
        if(obj.Length == 1)
        {
            base.Awake();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _userInfo = new UserInfo();
        _startPandaInfo = new StarterPandaInfo();
        _dialogueDatabase = new DialogueManager();
        _photoDatabase = new PhotoDatabase();
        _itemDatabase = new ItemDatabase();
        //_weatherDatabase = new WeatherApp();
        _recipeDatabase = new RecipeDatabase();
        _mbtiDatabase = new MBTIManager();

        _pandaDatabase = new PandaManager();
        _furniturePosDatabase = new FurniturePositionDatabase();
        _harvestItemDatabase = new HarvestItemManager();
        _npcDatabase = new NPCDatabase();
        _sideStoryDialogueDatabase = new SideStoryDialogueManager();
        _messageDatabase = new MessageDatabase();
        _albumDatabase = new AlbumDatabase();
        _challengesDatabase = new ChallengesDatabase();
        _challenges = new Challenges();

        UserInfo.Register();
        StartPandaInfo.Register();
        _dialogueDatabase.Register();
        _photoDatabase.Register();

        //Image
        //GatheringItem 
        for(int i = 0; i < _itemDatabase.GatheringItemSpriteArray.Length; i++)
        {
            _itemDatabase.GatheringItemSpriteArray[i] = _gatheringItemImages[i];
        }
        //ToolItem
        for (int i = 0; i < _itemDatabase.ToolItemSpriteArray.Length; i++)
        {
            _itemDatabase.ToolItemSpriteArray[i] = _toolItemImages[i];
        }

        _itemDatabase.FurnitureItemSprite = _furnitureItemImages;

        _itemDatabase.Register();

        //_weatherDatabase.Register();
        _recipeDatabase.Register();
        _mbtiDatabase.Register();

        _pandaDatabase.PandaImage = _pandaImage;
        _pandaDatabase.Register();
        _furniturePosDatabase.Register();

        _harvestItemDatabase.HarvestItemImage = _harvestItemImage;
        _harvestItemDatabase.Register();

        //Image
        for (int i = 0; i < _npcDatabase.NPCSpriteArray.Length; i++)
        {
            _npcDatabase.NPCSpriteArray[i] = _npcImages[i];
        }

        _npcDatabase.Register();
        _sideStoryDialogueDatabase.Register();
        _messageDatabase.MailPaperSpriteArray = _mailPaper;
        _messageDatabase.Register();

        _albumDatabase.AlbumSpriteArray = _albumImages;
        _albumDatabase.Register();

        _challengesDatabase.Register();
        _challenges.Register();
    }

    /// <summary>
    /// weekWeather data
    /// </summary>
    /// <returns></returns>
    public List<WeatherData> GetWeekWeatherData()
    {
        return _weatherDatabase.GetWeekWeathers();
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

    //Gathering Item
    /// <summary>
    /// GatheringItem BugList
    /// </summary>
    /// <returns></returns>
    public List<GatheringItem> GetBugItemList()
    {
        return _itemDatabase.ItemBugList;
    }

    /// <summary>
    /// GatheringItem FishList
    /// </summary>
    /// <returns></returns>
    public List<GatheringItem> GetFishItemList()
    {
        return _itemDatabase.ItemFishList;
    }

    /// <summary>
    /// GatheringItem FruitList
    /// </summary>
    /// <returns></returns>
    public List<GatheringItem> GetFruitItemList()
    {
        return _itemDatabase.ItemFruitList;
    }

    /// <summary>
    /// ToolItem List
    /// </summary>
    public List<ToolItem> GetGatheringToolItemList()
    {
        return _itemDatabase.ItemToolList;
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
        for(int i = 0; i < GetNPCList().Count; i++)
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

    public NPC GetNPC(string id)
    {
        for(int i=0;i<GetNPCList().Count; i++)
        {
            if (GetNPCList()[i].Id.Equals(id))
            {
                return GetNPCList()[i];
            }
        }
        return null;
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
    /// Album List
    /// </summary>
    /// <returns></returns>
    public List<Album> GetAlbumList()
    {
        return _albumDatabase.AlbumList;
    }

    public void SetReceiveAlbumById(string id)
    {
        for(int i=0;i< GetAlbumList().Count; i++)
        {
            if (GetAlbumList()[i].StoryStep.Equals(id))
            {
                GetAlbumList()[i].IsReceived = true;
                return;
            }
        }
    }


    /// <summary>
    /// 기존의 스티커 이미지 출력
    /// </summary>
    /// <returns></returns>
    public ItemSpriteDatabase GetStickerImage()
    {
        return _stickerImages;
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
