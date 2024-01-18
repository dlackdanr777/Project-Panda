using Muks.DataBind;
using System;
using System.Collections.Generic;
using UnityEngine;
using static FurnitureModel;

public class FurnitureModel
{
    public FurnitureId[] FurnitureRooms; // �� ����
    //public string[] FurnitureIds; // null�̸� �԰� �ִ� �� ����
    public bool IsExitFurniture;
    public bool IsSaveFurniture;
    public bool IsShowDetailView; // ����â�� ������� �ϸ� true

    public bool ChangedFurniture(Furniture furnitureData, ERoom room)
    {
        //if (furnitureData.Type == furnitureType)
        //{
            if (FurnitureRooms[(int)room].FurnitureIds[(int)furnitureData.Type] != "") // ������ ������ ���
            {

                // ������ ��ĥ ���
                if (FurnitureRooms[(int)room].FurnitureIds[(int)furnitureData.Type] == furnitureData.Id)
                {
                    // ���� ���� ���� ���
                    DataBind.SetTextValue("FurnitureDetailName", furnitureData.Name);
                    DataBind.SetTextValue("FurnitureDetailDescription", furnitureData.Description);
                    DataBind.SetSpriteValue("FurnitureDetailImage", furnitureData.Image);
                    Debug.Log("SetDetail");
                    return false;
                }
            }
        //}
        // ���� ��ġ�� ���� ID ����
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
        public string[] FurnitureIds; // ���� ��ġ�Ǿ� �ִ� ���� Id
        public FurnitureId()
        {
            FurnitureIds = new string[System.Enum.GetValues(typeof(FurnitureType)).Length - 1];
        }
    }
}
