using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : SingletonHandler<DatabaseManager>
{
    private UserInfo _userInfo;
    public UserInfo UserInfo;

    private PhotoDatabase _photoDatabase;
    public PhotoDatabase PhotoDatabase => _photoDatabase;

    private DialogueManager _dialogueDatabase;
    public DialogueManager DialogueDatabase => _dialogueDatabase;

    private WeatherApp _weatherDatabase;
    public WeatherApp WeatherDatabase => _weatherDatabase;

    private MBTIManager _mbtiDatabase;
    private PandaManager _pandaDatabase;
    private Database_Ssun _itemDatabase;

    public override void Awake()
    {
        base.Awake();

        _userInfo = new UserInfo();
        _dialogueDatabase = new DialogueManager();
        _photoDatabase = new PhotoDatabase();
        //_weatherDatabase = new WeatherApp();
        //_mbtiDatabase = new MBTIManager();
        //_pandaDatabase = new PandaManager();
        //_itemDatabase = new Database_Ssun();

        _userInfo.Register();
        _dialogueDatabase.Register();
        _photoDatabase.Register();
        //_weatherDatabase.Register();
        //_mbtiDatabase.Register();
        //_pandaDatabase.Register();
        //_itemDatabase.Register();



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
    public PandaData GetpandaData(int pandaID)
    {
        return _pandaDatabase.GetPandaData(pandaID);
    }

    /// <summary>
    /// panda image data
    /// </summary>
    /// <param name="pandaID">panda id</param>
    /// <returns></returns>
    public PandaStateImage GetpandaImage(int pandaID)
    {
        return _pandaDatabase.GetPandaImage(pandaID);
    }

    /// <summary>
    /// Toy ItemList
    /// </summary>
    /// <returns></returns>
    public List<Item> GeToyItem()
    {
        return _itemDatabase.ItemList[0];
    }

    /// <summary>
    /// Snack ItemList
    /// </summary>
    /// <returns></returns>
    public List<Item> GetSnackItem()
    {
        return _itemDatabase.ItemList[1];
    }

    /// <summary>
    /// Furniture ItemList
    /// </summary>
    /// <returns></returns>
    public List<Item> GetFurnitureItem()
    {
        return _itemDatabase.ItemList[2];
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
        _userInfo.SaveUserInfoData();
    }
}
