using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeModel
{
    public int WearingHeadCostumeID; // -1이면 입고 있는 옷 없음
    public bool IsExitCostume;
    public bool IsSaveCostume;

    public bool WearingCostume(CostumeData costumeData)
    {
        if(costumeData.BodyParts == EBodyParts.Head)
        {
            if (WearingHeadCostumeID != -1) // 입고 있는 옷이 있을 경우
            {
                CostumeManager.Instance.CostumeDic[WearingHeadCostumeID].CostumeSlot.SetActive(false); // 입고 있는 옷 벗기

                // 옷이 겹칠 경우
                if (CostumeManager.Instance.CostumeDic[WearingHeadCostumeID].CostumeID == costumeData.CostumeID)
                {
                    // 현재 입고 있는 옷 해제
                    //WearingHeadCostumeID = -1;
                    return false;
                }
            }
            // 현재 입고 있는 옷 ID 저장
            //WearingHeadCostumeID = costumeData.CostumeID;
            costumeData.CostumeSlot.SetActive(true);
        }
        return true;
        
    }

    public void Init()
    {
        WearingHeadCostumeID = -1;
        Debug.Log("판다 모델 초기화 WearingHeadCostumeID: " + WearingHeadCostumeID);
    }


    public void SaveCostume()
    {
        DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID = WearingHeadCostumeID;
    }
}
