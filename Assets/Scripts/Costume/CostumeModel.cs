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
            if (WearingHeadCostumeID != "") // �԰� �ִ� ���� ���� ���
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
        WearingHeadCostumeID = "";
        Debug.Log("�Ǵ� �� �ʱ�ȭ WearingHeadCostumeID: " + WearingHeadCostumeID);
    }


    public void SaveCostume()
    {
        // ���� �԰� �ִ� �� ����
        string wearingHeadCostumeID = DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID;
        if (wearingHeadCostumeID != "")
        {
            CostumeManager.Instance.CostumeDic[wearingHeadCostumeID].CostumeSlot.SetActive(false);
        }

        // ������ �� �Ա�
        DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID = WearingHeadCostumeID;
        if (WearingHeadCostumeID != "")
        {
            CostumeManager.Instance.CostumeDic[WearingHeadCostumeID].CostumeSlot.SetActive(true);
        }
    }
}
