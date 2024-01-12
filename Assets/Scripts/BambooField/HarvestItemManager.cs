using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestItemManager
{
    private Dictionary<string, HarvestItem> _harvestItemDic;
    public HarvestItemImage HarvestItemImage{ private get; set; }

    public void Register()
    {
        _harvestItemDic = HarvestItemParse("HarvestItem");
        
        // ��Ȯ������ �̹��� ����
        foreach(string id in _harvestItemDic.Keys)
        {
            _harvestItemDic[id].Image[0] = HarvestItemImage.GrowthStageImages[0].ZeroStepImage;
            _harvestItemDic[id].Image[1] = HarvestItemImage.GrowthStageImages[0].OneStepImage;
            _harvestItemDic[id].Image[2] = HarvestItemImage.GrowthStageImages[0].TwoStepImage;
        }
    }

    /// <summary>
    /// ��Ȯ ������ ������ �޾ƿ� ���� </summary>
    public Dictionary<string, HarvestItem> HarvestItemParse(string CSVFileName)
    {
        Dictionary<string, HarvestItem> harvestItemDic = new Dictionary<string, HarvestItem>();

        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            harvestItemDic.Add(row[0], new HarvestItem(row[0], row[1], int.Parse(row[2]), int.Parse(row[3]), int.Parse(row[4]), row[5]));
        }
        return harvestItemDic;
    }

    public HarvestItem GetHarvestItemdata(string harvestItemID)
    {
        HarvestItem harvestItem = _harvestItemDic[harvestItemID];
        return harvestItem;
    }
}
