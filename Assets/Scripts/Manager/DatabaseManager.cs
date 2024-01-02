using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class DatabaseManager : SingletonHandler<DatabaseManager>
{
    private UserInfo _userInfo;
    public UserInfo UserInfo => _userInfo;

    private PhotoDatabase _photoDatabase;
    public PhotoDatabase PhotoDatabase => _photoDatabase;

    private DialogueManager _dialogueDatabase;
    public DialogueManager DialogueDatabase => _dialogueDatabase;

    private WeatherApp _weatherDatabase;
    public WeatherApp WeatherDatabase => _weatherDatabase;

    private MBTIManager _mbtiDatabase;

    [SerializeField] private PandaImage _pandaImage;
    private PandaManager _pandaDatabase;

    //Item
    [SerializeField] private ItemSpriteDatabase[] _gatheringItemImages;
    private ItemDatabase _itemDatabase;
    public ItemDatabase ItemDatabase => _itemDatabase;
    
    private FurniturePositionDatabase _furniturePosDatabase;
    public FurniturePositionDatabase FurniturePosDatabase => _furniturePosDatabase;

    private HarvestItemManager _harvestItemDatabase;
    [SerializeField] HarvestItemImage _harvestItemImage;

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
        _dialogueDatabase = new DialogueManager();
        _photoDatabase = new PhotoDatabase();
        _itemDatabase = new ItemDatabase();
        //_weatherDatabase = new WeatherApp();
        _mbtiDatabase = new MBTIManager();


        _pandaDatabase = new PandaManager();
        _furniturePosDatabase = new FurniturePositionDatabase();
        _harvestItemDatabase = new HarvestItemManager();

        UserInfo.Register();
        _dialogueDatabase.Register();
        _photoDatabase.Register();

        for(int i = 0; i < _itemDatabase.ItemSpriteArray.Length; i++)
        {
            _itemDatabase.ItemSpriteArray[i] = _gatheringItemImages[i];
        }
        _itemDatabase.Register();

        //_weatherDatabase.Register();
        _mbtiDatabase.Register();

        _pandaDatabase.PandaImage = _pandaImage;
        _pandaDatabase.Register();
        _furniturePosDatabase.Register();

        _harvestItemDatabase.HarvestItemImage = _harvestItemImage;
        _harvestItemDatabase.Register();

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
    public List<Item> GetBugItemList()
    {
        return _itemDatabase.ItemBugList;
    }

    /// <summary>
    /// GatheringItem FishList
    /// </summary>
    /// <returns></returns>
    public List<Item> GetFishItemList()
    {
        return _itemDatabase.ItemFishList;
    }

    /// <summary>
    /// GatheringItem FruitList
    /// </summary>
    /// <returns></returns>
    public List<Item> GetFruitItemList()
    {
        return _itemDatabase.ItemFruitList;
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


    public void OnApplicationQuit()
    {
        _photoDatabase.Save();
        _furniturePosDatabase.Save();
        _userInfo.SaveUserInfoData();
    }

    public HarvestItem GetHarvestItemdata(int harvestItemID)
    {
        return _harvestItemDatabase.GetHarvestItemdata(harvestItemID);
    }
}
