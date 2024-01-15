using Muks.DataBind;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureModel
{
    public string[] FurnitureId; // null�̸� �԰� �ִ� �� ����
    public bool IsExitFurniture;
    public bool IsSaveFurniture;
    public bool IsShowDetailView; // ����â�� ������� �ϸ� true

    public bool ChangedFurniture(Furniture furnitureData)
    {
        //if (furnitureData.Type == furnitureType)
        //{
            if (FurnitureId[(int)furnitureData.Type] != "") // ������ ������ ���
            {

                // ������ ��ĥ ���
                if (FurnitureId[(int)furnitureData.Type] == furnitureData.Id)
                {
                    // ���� ���� ���� ���
                    DataBind.SetTextValue("FurnitureDetailName", furnitureData.Name);
                    DataBind.SetTextValue("FurnitureDetailDescription", furnitureData.Description);
                    DataBind.SetSpriteValue("FurnitureDetailImage", furnitureData.Image);
                    return false;
                }
            }
        //}
        // ���� ��ġ�� ���� ID ����
        return true;

    }

    public void Init()
    {
        FurnitureId = new string[System.Enum.GetValues(typeof(FurnitureType)).Length - 1];
        for (int i = 0; i < FurnitureId.Length; i++)
        {
            FurnitureId[i] = "";
        }
        Debug.Log("�Ǵ� �� �ʱ�ȭ WallPaperID: " + FurnitureId);
    }


    public void SaveFurniture()
    {
        // ���� ���� ��� �����ϱ�
        //// ���� �԰� �ִ� �� ����
        //string wallPaperId = DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID;
        //if (wallPaperId != "")
        //{
        //    CostumeManager.Instance.CostumeDic[wallPaperId].CostumeSlot.SetActive(false);
        //}

        //// ������ �� �Ա�
        //DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID = WallPaperID;
        //if (WallPaperID != "")
        //{
        //    CostumeManager.Instance.CostumeDic[WallPaperID].CostumeSlot.SetActive(true);
        //}
    }
}
