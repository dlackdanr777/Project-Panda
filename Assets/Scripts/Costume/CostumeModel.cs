using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeModel
{
    public int WearingHeadCostumeID; // -1�̸� �԰� �ִ� �� ����
    private StarterPandaInfo _starterPandaInfo;

    public void WearingCostume(CostumeData costumeData)
    {
        if(costumeData.BodyParts == EBodyParts.Head)
        {
            if (WearingHeadCostumeID != -1) // ���� ��ĥ ���
            {
                CostumeManager.Instance.CostumeDic[WearingHeadCostumeID].CostumeSlot.SetActive(false);
            }
            if (CostumeManager.Instance.CostumeDic[WearingHeadCostumeID].CostumeID == costumeData.CostumeID)
            {
                // ���� �԰� �ִ� �� ����
                WearingHeadCostumeID = -1;
            }
            // ���� �԰� �ִ� �� ID ����
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
