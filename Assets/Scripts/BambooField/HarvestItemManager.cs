using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestItemManager : SingletonHandler<HarvestItemManager>
{
    private Dictionary<int, HarvestItem> _harvestItemDic;
    [SerializeField] private HarvestItemImage _harvestItemImage;

    public override void Awake()
    {
        base.Awake();
        _harvestItemDic = HarvestItemParse("HarvestItem");
        
        // 수확아이템 이미지 저장
        for(int i = 0; i < _harvestItemDic.Count; i++)
        {
            _harvestItemDic[i].Image[0] = _harvestItemImage.GrowthStageImages[i].ZeroStepImage;
            _harvestItemDic[i].Image[1] = _harvestItemImage.GrowthStageImages[i].OneStepImage;
            _harvestItemDic[i].Image[2] = _harvestItemImage.GrowthStageImages[i].TwoStepImage;
        }
    }

    /// <summary>
    /// 수확 아이템 데이터 받아와 저장 </summary>
    public Dictionary<int, HarvestItem> HarvestItemParse(string CSVFileName)
    {
        Dictionary<int, HarvestItem> harvestItemDic = new Dictionary<int, HarvestItem>();

        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            harvestItemDic.Add(int.Parse(row[0]), new HarvestItem(int.Parse(row[0]), row[1], float.Parse(row[2]), int.Parse(row[3]), int.Parse(row[4]), row[5]));
        }
        return harvestItemDic;
    }

    public HarvestItem GetHarvestItemdata(int harvestItemID)
    {
        HarvestItem harvestItem = _harvestItemDic[harvestItemID];
        return harvestItem;
    }
}
