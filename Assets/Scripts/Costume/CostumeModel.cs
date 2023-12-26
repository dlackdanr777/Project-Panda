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

                // ���� ��ĥ ���
                if (CostumeManager.Instance.CostumeDic[WearingHeadCostumeID].CostumeID == costumeData.CostumeID)
                {
                    // ���� �԰� �ִ� �� ����
                    return false;
                }
            }
        }
        // ���� �԰� �ִ� �� ID ����
        return true;
        
    }

    public void Init()
    {
        WearingHeadCostumeID = -1;
        Debug.Log("�Ǵ� �� �ʱ�ȭ WearingHeadCostumeID: " + WearingHeadCostumeID);
    }


    public void SaveCostume()
    {
        // ���� �԰� �ִ� �� ����
        int wearingHeadCostumeID = DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID;
        if(wearingHeadCostumeID != -1)
        {
            CostumeManager.Instance.CostumeDic[wearingHeadCostumeID].CostumeSlot.SetActive(false);
        }

        // ������ �� �Ա�
        DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID = WearingHeadCostumeID;
        if (WearingHeadCostumeID != -1)
        {
            CostumeManager.Instance.CostumeDic[WearingHeadCostumeID].CostumeSlot.SetActive(true);
        }
    }
}
