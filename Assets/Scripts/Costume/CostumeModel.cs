using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeModel
{
    public int WearingHeadCostumeID; // -1�̸� �԰� �ִ� �� ����
    public bool IsExitCostume;
    public bool IsSaveCostume;

    public bool WearingCostume(CostumeData costumeData)
    {
        if(costumeData.BodyParts == EBodyParts.Head)
        {
            if (WearingHeadCostumeID != -1) // �԰� �ִ� ���� ���� ���
            {
                CostumeManager.Instance.CostumeDic[WearingHeadCostumeID].CostumeSlot.SetActive(false); // �԰� �ִ� �� ����

                // ���� ��ĥ ���
                if (CostumeManager.Instance.CostumeDic[WearingHeadCostumeID].CostumeID == costumeData.CostumeID)
                {
                    // ���� �԰� �ִ� �� ����
                    //WearingHeadCostumeID = -1;
                    return false;
                }
            }
            // ���� �԰� �ִ� �� ID ����
            //WearingHeadCostumeID = costumeData.CostumeID;
            costumeData.CostumeSlot.SetActive(true);
        }
        return true;
        
    }

    public void Init()
    {
        WearingHeadCostumeID = -1;
        Debug.Log("�Ǵ� �� �ʱ�ȭ WearingHeadCostumeID: " + WearingHeadCostumeID);
    }


    public void SaveCostume()
    {
        DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID = WearingHeadCostumeID;
    }
}
