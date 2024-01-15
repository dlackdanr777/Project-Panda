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
    [SerializeField] private ItemSpriteDatabase[] _npcImages;
    private ItemDatabase _itemDatabase;
    public ItemDatabase ItemDatabase => _itemDatabase;
    
    private FurniturePositionDatabase _furniturePosDatabase;
    public FurniturePositionDatabase FurniturePosDatabase => _furniturePosDatabase;

    private HarvestItemManager _harvestItemDatabase;
    [SerializeField] HarvestItemImage _harvestItemImage;

    private NPCDatabase _npcDatabase;
    public NPCDatabase NPCDatabase => _npcDatabase;

    //Message
    [SerializeField] private ItemSpriteDatabase _mailPaper;
    private MessageDatabase _messageDatabase;
    public MessageDatabase MessageDatabase => _messageDatabase;

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
        _messageDatabase = new MessageDatabase();


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
        _messageDatabase.MailPaperSpriteArray = _mailPaper;
        _messageDatabase.Register();

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
    /// mbti에 따른 판다 취향 설정
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
    /// 판다 친밀도 업데이트 </summary>
    public void UpdatePandaIntimacy(int pandaID, float intimacy)
    {
        _pandaDatabase.UpdatePandaIntimacy(pandaID, intimacy);
    }

    /// <summary>
    /// 판다 행복도 업데이트 </summary>
    public void UpdatePandaHappiness(int pandaID, float happiness)
    {
        _pandaDatabase.UpdatePandaHappiness(pandaID, happiness);
    }

    /// <summary>
    /// 스타터 판다 mbti 설정</summary>
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
    /// Furniture ItemList
    /// </summary>
    /// <returns></returns>
    public List<Item> GetFurnitureItem()
    {
        return _itemDatabase.FurnitureList;
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

    public void OnApplicationQuit()
    {
        _photoDatabase.Save();
        _furniturePosDatabase.Save();
        _userInfo.SaveUserInfoData();
        _startPandaInfo.SavePandaInfoData();
    }

    public HarvestItem GetHarvestItemdata(string harvestItemID)
    {
        return _harvestItemDatabase.GetHarvestItemdata(harvestItemID);
    }
}
