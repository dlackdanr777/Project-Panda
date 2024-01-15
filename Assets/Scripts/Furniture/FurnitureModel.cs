using Muks.DataBind;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureModel
{
    public string[] FurnitureId; // null이면 입고 있는 옷 없음
    public bool IsExitFurniture;
    public bool IsSaveFurniture;
    public bool IsShowDetailView; // 설명창을 보여줘야 하면 true

    public bool ChangedFurniture(Furniture furnitureData)
    {
        //if (furnitureData.Type == furnitureType)
        //{
            if (FurnitureId[(int)furnitureData.Type] != "") // 가구가 존재할 경우
            {

                // 가구가 겹칠 경우
                if (FurnitureId[(int)furnitureData.Type] == furnitureData.Id)
                {
                    // 현재 가구 정보 띄움
                    DataBind.SetTextValue("FurnitureDetailName", furnitureData.Name);
                    DataBind.SetTextValue("FurnitureDetailDescription", furnitureData.Description);
                    DataBind.SetSpriteValue("FurnitureDetailImage", furnitureData.Image);
                    return false;
                }
            }
        //}
        // 현재 배치한 가구 ID 저장
        return true;

    }

    public void Init()
    {
        FurnitureId = new string[System.Enum.GetValues(typeof(FurnitureType)).Length - 1];
        for (int i = 0; i < FurnitureId.Length; i++)
        {
            FurnitureId[i] = "";
        }
        Debug.Log("판다 모델 초기화 WallPaperID: " + FurnitureId);
    }


    public void SaveFurniture()
    {
        // 가구 저장 기능 수정하기
        //// 지금 입고 있던 옷 벗기
        //string wallPaperId = DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID;
        //if (wallPaperId != "")
        //{
        //    CostumeManager.Instance.CostumeDic[wallPaperId].CostumeSlot.SetActive(false);
        //}

        //// 저장한 옷 입기
        //DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID = WallPaperID;
        //if (WallPaperID != "")
        //{
        //    CostumeManager.Instance.CostumeDic[WallPaperID].CostumeSlot.SetActive(true);
        //}
    }
}
