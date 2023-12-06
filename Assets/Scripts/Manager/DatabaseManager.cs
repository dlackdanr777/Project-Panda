using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : SingletonHandler<DatabaseManager>
{
    private DataList<PhotoData> _photoDatabase;

    private DialogueManager _dialogueDatabase;
    private WeatherApp _weatherDatabase;
    private MBTIManager _mbtiDatabase;
    private PandaManager _pandaDatabase;
    private Database_Ssun _itemDatabase;

    public override void Awake()
    {
        base.Awake();
        _dialogueDatabase.Awake();
        _weatherDatabase.Awake();
        _mbtiDatabase.Awake();
        _pandaDatabase.Awake();
        _itemDatabase.Awake();    

        _photoDatabase = new DataList<PhotoData>();
    }


    /// <summary>
    /// 해당 ID의 Dialogue를 가져옴
    /// </summary>
    /// <param name="index">story ID</param>
    public StoryDialogue GetDialogueData(int index)
    {
        return _dialogueDatabase.GetStoryDialogue(index);
    }
    
    /// <summary>
    /// photo data
    /// </summary>
    /// <param name="index">indeㅌ</param>
    /// <returns></returns>
    public DataList<PhotoData> GetPhotoData(int index)
    {
        return _photoDatabase;
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

}
