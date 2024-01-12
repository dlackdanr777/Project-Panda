using Muks.DataBind;
using System;
using UnityEngine;

public class FurnitureModel
{
    public string WallID; // null이면 입고 있는 옷 없음
    public bool IsExitFurniture;
    public bool IsSaveFurniture;
    public bool IsShowDetailView; // 설명창을 보여줘야 하면 true

    public bool ChangedFurniture(CostumeData costumeData)
    {
        if (costumeData.BodyParts == EBodyParts.Head)
        {
            if (WallID != "") // 가구가 존재할 경우
            {

                // 가구가 겹칠 경우
                if (CostumeManager.Instance.CostumeDic[WallID].CostumeID == costumeData.CostumeID)
                {
                    // 현재 가구 정보 띄움
                    DataBind.SetTextValue("FurnitureDetailName", costumeData.CostumeName);
                    //DataBind.SetTextValue("FurnitureDetailDescription", costumeData.Description);
                    DataBind.SetSpriteValue("FurnitureDetailImage", costumeData.Image);
                    return false;
                }
            }
        }
        // 현재 배치한 가구 ID 저장
        return true;

    }

    public void Init()
    {
        WallID = "";
        Debug.Log("판다 모델 초기화 WearingHeadCostumeID: " + WallID);
    }


    public void SaveFurniture()
    {
        // 지금 입고 있던 옷 벗기
        string wearingHeadCostumeID = DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID;
        if (wearingHeadCostumeID != "")
        {
            CostumeManager.Instance.CostumeDic[wearingHeadCostumeID].CostumeSlot.SetActive(false);
        }

        // 저장한 옷 입기
        DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID = WallID;
        if (WallID != "")
        {
            CostumeManager.Instance.CostumeDic[WallID].CostumeSlot.SetActive(true);
        }
    }
}
