using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeManager : SingletonHandler<CostumeManager>
{
    public Dictionary<string, CostumeData> CostumeDic;
    [SerializeField] private CostumeImage _costumeImage;
    [SerializeField] private GameObject _costumeSlot;
    [SerializeField] private GameObject _pandaCostume;

    public override void Awake()
    {
        var obj = FindObjectsOfType<CostumeManager>();
        if (obj.Length == 1)
        {
            base.Awake();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        CostumeDic = CostumeParse("Costume");
        int headCount = 0, leftHandCount = 0, rightHandCount = 0;

        foreach (string key in CostumeDic.Keys)
        {
            // 코스튬 판다 부위에 맞추어 생성
            GameObject costumeSlot = Instantiate(_costumeSlot, _pandaCostume.transform.GetChild((int)CostumeDic[key].BodyParts));
            
            // 코스튬 위치 지정
            costumeSlot.transform.position = _pandaCostume.transform.GetChild((int)CostumeDic[key].BodyParts).transform.position + CostumeDic[key].CostumePosition;

            if (CostumeDic[key].BodyParts == EBodyParts.Head) CostumeDic[key].Image = _costumeImage.HeadCostumeImages[headCount++];
            else if (CostumeDic[key].BodyParts == EBodyParts.LeftHand) CostumeDic[key].Image = _costumeImage.LeftHandCostumeImages[leftHandCount++];
            else if (CostumeDic[key].BodyParts == EBodyParts.RightHand) CostumeDic[key].Image = _costumeImage.RightCostumeImages[rightHandCount++];

            costumeSlot.GetComponent<SpriteRenderer>().sprite = CostumeDic[key].Image;
            CostumeDic[key].CostumeSlot = costumeSlot;
            costumeSlot.SetActive(false);
        }

        // 현재 가지고 있는 코스튬 불러오기
        DatabaseManager.Instance.StartPandaInfo.LoadMyCostume();
    }

    private Dictionary<string, CostumeData> CostumeParse(string CSVFileName)
    {
        Dictionary<string, CostumeData> costumeDic = new Dictionary<string, CostumeData>();

        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            costumeDic.Add(row[0], new CostumeData(row[0], (EBodyParts)Enum.Parse(typeof(EBodyParts), row[1]), row[2], float.Parse(row[3]), float.Parse(row[4]), float.Parse(row[5])));
        }
        return costumeDic;
    }

    public CostumeData GetCostumeData(string costumeID) 
    {
        CostumeData costumeData = CostumeDic[costumeID];
        return costumeData;
    }
}
