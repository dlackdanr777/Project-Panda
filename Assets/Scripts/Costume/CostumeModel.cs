using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeModel
{
    public string WearingHeadCostumeID; // null이면 입고 있는 옷 없음
    public bool IsExitCostume;
    public bool IsSaveCostume;

    public bool WearingCostume(CostumeData costumeData)
    {
        if(costumeData.BodyParts == EBodyParts.Head)
        {
            if (WearingHeadCostumeID != null) // 입고 있는 옷이 있을 경우
            {

                // 옷이 겹칠 경우
                if (CostumeManager.Instance.CostumeDic[WearingHeadCostumeID].CostumeID == costumeData.CostumeID)
                {
                    // 현재 입고 있는 옷 해제
                    return false;
                }
            }
        }
        // 현재 입고 있는 옷 ID 저장
        return true;
        
    }

    public void Init()
    {
        WearingHeadCostumeID = null;
        Debug.Log("판다 모델 초기화 WearingHeadCostumeID: " + WearingHeadCostumeID);
    }


    public void SaveCostume()
    {
        // 지금 입고 있던 옷 벗기
        string wearingHeadCostumeID = DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID;
        if(wearingHeadCostumeID != null)
        {
            CostumeManager.Instance.CostumeDic[wearingHeadCostumeID].CostumeSlot.SetActive(false);
        }

        // 저장한 옷 입기
        DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID = WearingHeadCostumeID;
        if (WearingHeadCostumeID != null)
        {
            CostumeManager.Instance.CostumeDic[WearingHeadCostumeID].CostumeSlot.SetActive(true);
        }
    }
}
