using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeModel
{
    public string WearingHeadCostumeID; // null�̸� �԰� �ִ� �� ����
    public bool IsExitCostume;
    public bool IsSaveCostume;

    public bool WearingCostume(CostumeData costumeData)
    {
        if(costumeData.BodyParts == EBodyParts.Head)
        {
            if (WearingHeadCostumeID != null) // �԰� �ִ� ���� ���� ���
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
        WearingHeadCostumeID = null;
        Debug.Log("�Ǵ� �� �ʱ�ȭ WearingHeadCostumeID: " + WearingHeadCostumeID);
    }


    public void SaveCostume()
    {
        // ���� �԰� �ִ� �� ����
        string wearingHeadCostumeID = DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID;
        if(wearingHeadCostumeID != null)
        {
            CostumeManager.Instance.CostumeDic[wearingHeadCostumeID].CostumeSlot.SetActive(false);
        }

        // ������ �� �Ա�
        DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID = WearingHeadCostumeID;
        if (WearingHeadCostumeID != null)
        {
            CostumeManager.Instance.CostumeDic[WearingHeadCostumeID].CostumeSlot.SetActive(true);
        }
    }
}
