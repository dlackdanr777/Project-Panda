using Muks.DataBind;
using System;
using UnityEngine;

public class FurnitureModel
{
    public string WallID; // null�̸� �԰� �ִ� �� ����
    public bool IsExitFurniture;
    public bool IsSaveFurniture;
    public bool IsShowDetailView; // ����â�� ������� �ϸ� true

    public bool ChangedFurniture(CostumeData costumeData)
    {
        if (costumeData.BodyParts == EBodyParts.Head)
        {
            if (WallID != "") // ������ ������ ���
            {

                // ������ ��ĥ ���
                if (CostumeManager.Instance.CostumeDic[WallID].CostumeID == costumeData.CostumeID)
                {
                    // ���� ���� ���� ���
                    DataBind.SetTextValue("FurnitureDetailName", costumeData.CostumeName);
                    //DataBind.SetTextValue("FurnitureDetailDescription", costumeData.Description);
                    DataBind.SetSpriteValue("FurnitureDetailImage", costumeData.Image);
                    return false;
                }
            }
        }
        // ���� ��ġ�� ���� ID ����
        return true;

    }

    public void Init()
    {
        WallID = "";
        Debug.Log("�Ǵ� �� �ʱ�ȭ WearingHeadCostumeID: " + WallID);
    }


    public void SaveFurniture()
    {
        // ���� �԰� �ִ� �� ����
        string wearingHeadCostumeID = DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID;
        if (wearingHeadCostumeID != "")
        {
            CostumeManager.Instance.CostumeDic[wearingHeadCostumeID].CostumeSlot.SetActive(false);
        }

        // ������ �� �Ա�
        DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID = WallID;
        if (WallID != "")
        {
            CostumeManager.Instance.CostumeDic[WallID].CostumeSlot.SetActive(true);
        }
    }
}
