using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeModel
{
    public bool[] IsWearing;
    public int[] WearingCostumeID;

    public void WearingCostume(CostumeData costumeData)
    {
        if (IsWearing[(int)costumeData.BodyParts]) // ¿ÊÀÌ °ãÄ¥ °æ¿ì
        {
            CostumeManager.Instance.CostumeDic[WearingCostumeID[(int)costumeData.BodyParts]].CostumeSlot.SetActive(false);
        }
        if(CostumeManager.Instance.CostumeDic[WearingCostumeID[(int)costumeData.BodyParts]].CostumeID == costumeData.CostumeID)
        {
            WearingCostumeID[(int)costumeData.BodyParts] = -1;
        }
        // ÇöÀç ÀÔ°í ÀÖ´Â ¿Ê ID ÀúÀå
        WearingCostumeID[(int)costumeData.BodyParts] = costumeData.CostumeID;
        costumeData.CostumeSlot.SetActive(true);
    }

    public void Init()
    {
        IsWearing = new bool[CostumeManager.Instance.CostumeDic.Count];
        WearingCostumeID = new int[CostumeManager.Instance.CostumeDic.Count];
        for (int i = 0; i<CostumeManager.Instance.CostumeDic.Count; i++)
        {
            WearingCostumeID[i] = -1;
        }
    }
}
