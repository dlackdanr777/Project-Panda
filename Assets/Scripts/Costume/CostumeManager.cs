using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeManager : SingletonHandler<CostumeManager>
{
    public Dictionary<int, CostumeData> CostumeDic;
    [SerializeField] private CostumeImage _costumeImage;
    [SerializeField] private GameObject _costumeSlot;
    [SerializeField] private GameObject _pandaCostume;

    public override void Awake()
    {
        base.Awake();
        CostumeDic = CostumeParse("Costume");
        int headCount = 0, leftHandCount = 0, rightHandCount = 0;

        for (int i = 0; i < CostumeDic.Count; i++)
        {
            // 코스튬 판다 부위에 맞추어 생성
            GameObject costumeSlot = Instantiate(_costumeSlot, _pandaCostume.transform.GetChild((int)CostumeDic[i].BodyParts));

            if (CostumeDic[i].BodyParts == EBodyParts.Head) CostumeDic[i].Image = _costumeImage.HeadCostumeImages[headCount++];
            else if (CostumeDic[i].BodyParts == EBodyParts.LeftHand) CostumeDic[i].Image = _costumeImage.LeftHandCostumeImages[leftHandCount++];
            else if (CostumeDic[i].BodyParts == EBodyParts.RightHand) CostumeDic[i].Image = _costumeImage.RightCostumeImages[rightHandCount++];

            costumeSlot.GetComponent<SpriteRenderer>().sprite = CostumeDic[i].Image;
            CostumeDic[i].CostumeSlot = costumeSlot;
            costumeSlot.SetActive(false);
        }

    }

    private Dictionary<int, CostumeData> CostumeParse(string CSVFileName)
    {
        Dictionary<int, CostumeData> costumeDic = new Dictionary<int, CostumeData>();

        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            costumeDic.Add(int.Parse(row[0]), new CostumeData(int.Parse(row[0]), (EBodyParts)Enum.Parse(typeof(EBodyParts), row[1]), row[2], float.Parse(row[3]), float.Parse(row[4]), float.Parse(row[5])));
        }
        return costumeDic;
    }

    public CostumeData GetCostumeData(int costumeID) 
    {
        CostumeData costumeData = CostumeDic[costumeID];
        return costumeData;
    }
}
