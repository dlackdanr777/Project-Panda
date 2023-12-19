using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeModel
{
    public int WearingHeadCostumeID; // -1이면 입고 있는 옷 없음
    private StarterPandaInfo _starterPandaInfo;

    public void WearingCostume(CostumeData costumeData)
    {
        if(costumeData.BodyParts == EBodyParts.Head)
        {
            if (WearingHeadCostumeID != -1) // 옷이 겹칠 경우
            {
                CostumeManager.Instance.CostumeDic[WearingHeadCostumeID].CostumeSlot.SetActive(false);
            }
            if (CostumeManager.Instance.CostumeDic[WearingHeadCostumeID].CostumeID == costumeData.CostumeID)
            {
                // 현재 입고 있는 옷 해제
                WearingHeadCostumeID = -1;
            }
            // 현재 입고 있는 옷 ID 저장
            WearingHeadCostumeID = costumeData.CostumeID;
            costumeData.CostumeSlot.SetActive(true);
        }
        
    }

    public void Init()
    {
        WearingHeadCostumeID = -1;
    }


    public void SaveCostume()
    {
        _starterPandaInfo.WearingHeadCostumeID = WearingHeadCostumeID;
    }
}
