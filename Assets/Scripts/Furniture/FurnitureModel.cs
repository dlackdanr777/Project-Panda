using Muks.DataBind;
using System;
using System.Collections.Generic;
using UnityEngine;
using static FurnitureModel;

public class FurnitureModel
{
    public FurnitureId[] FurnitureRooms; // 방 설정
    //public string[] FurnitureIds; // null이면 입고 있는 옷 없음
    public bool IsExitFurniture;
    public bool IsSaveFurniture;
    public bool IsShowDetailView; // 설명창을 보여줘야 하면 true

    public bool ChangedFurniture(Furniture furnitureData, ERoom room)
    {
        //if (furnitureData.Type == furnitureType)
        //{
            if (FurnitureRooms[(int)room].FurnitureIds[(int)furnitureData.Type] != "") // 가구가 존재할 경우
            {

                // 가구가 겹칠 경우
                if (FurnitureRooms[(int)room].FurnitureIds[(int)furnitureData.Type] == furnitureData.Id)
                {
                    // 현재 가구 정보 띄움
                    DataBind.SetTextValue("FurnitureDetailName", furnitureData.Name);
                    DataBind.SetTextValue("FurnitureDetailDescription", furnitureData.Description);
                    DataBind.SetSpriteValue("FurnitureDetailImage", furnitureData.Image);
                    Debug.Log("SetDetail");
                    return false;
                }
            }
        //}
        // 현재 배치한 가구 ID 저장
        return true;

    }

    public void Init()
    {
        FurnitureRooms = new FurnitureId[System.Enum.GetValues(typeof(ERoom)).Length];
        for (int i = 0; i < FurnitureRooms.Length; i++)
        {
            FurnitureRooms[i] = new FurnitureId();

            for (int j = 0; j < FurnitureRooms[i].FurnitureIds.Length; j++)
            {
                FurnitureRooms[i].FurnitureIds[j] = "";
            }
        }
        
    }


    public void SaveFurniture()
    {
        DatabaseManager.Instance.StartPandaInfo.FurnitureRooms = FurnitureRooms;
    }

    [Serializable]
    public class FurnitureId
    {
        public string[] FurnitureIds; // 현재 배치되어 있는 가구 Id
        public FurnitureId()
        {
            FurnitureIds = new string[System.Enum.GetValues(typeof(FurnitureType)).Length - 1];
        }
    }
}
