using BackEnd;
using LitJson;
using Muks.BackEnd;
using Muks.DataBind;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CostumeManager : SingletonHandler<CostumeManager>
{
    public Dictionary<string, CostumeData> CostumeDic = new Dictionary<string, CostumeData>();
    [SerializeField] private CostumeImage _costumeImage;
    [SerializeField] private GameObject _costumeSlot;
    [SerializeField] private GameObject _pandaCostume;

    public override void Awake()
    {
        var obj = FindObjectsOfType<CostumeManager>();
        if (obj.Length == 1)
        {
            base.Awake();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if(CostumeDic.Count <= 0)
            CostumeDic = CostumeParserByLocal("Costume");

        int headCount = 0, leftHandCount = 0, rightHandCount = 0;

        foreach (string key in CostumeDic.Keys)
        {
            // �ڽ�Ƭ �Ǵ� ������ ���߾� ����
            GameObject costumeSlot = Instantiate(_costumeSlot, _pandaCostume.transform.GetChild((int)CostumeDic[key].BodyParts));
            
            // �ڽ�Ƭ ��ġ ����
            costumeSlot.transform.position = _pandaCostume.transform.GetChild((int)CostumeDic[key].BodyParts).transform.position + CostumeDic[key].CostumePosition;

            if (CostumeDic[key].BodyParts == EBodyParts.Head) CostumeDic[key].Image = _costumeImage.HeadCostumeImages[headCount++];
            else if (CostumeDic[key].BodyParts == EBodyParts.LeftHand) CostumeDic[key].Image = _costumeImage.LeftHandCostumeImages[leftHandCount++];
            else if (CostumeDic[key].BodyParts == EBodyParts.RightHand) CostumeDic[key].Image = _costumeImage.RightCostumeImages[rightHandCount++];

            costumeSlot.GetComponent<SpriteRenderer>().sprite = CostumeDic[key].Image;
            CostumeDic[key].CostumeSlot = costumeSlot;
            costumeSlot.SetActive(false);
        }

        // ���� ������ �ִ� �ڽ�Ƭ �ҷ�����
        DatabaseManager.Instance.StartPandaInfo.LoadMyCostume();
        DataBind.SetButtonValue("CostumeSceneButton", () => LoadingSceneManager.LoadScene("CostumeTest"));
    }


    public void LoadData()
    {
        BackendManager.Instance.GetChartData("105907", 10, CostumeParserByServer);
    }


    /// <summary>���ҽ� �������� �ڽ�Ƭ ������ �޾ƿ� ��ųʸ��� �ִ� �Լ�</summary>
    private Dictionary<string, CostumeData> CostumeParserByLocal(string CSVFileName)
    {
        Dictionary<string, CostumeData> costumeDic = new Dictionary<string, CostumeData>();

        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            costumeDic.Add(row[0], new CostumeData(row[0], (EBodyParts)Enum.Parse(typeof(EBodyParts), row[1]), row[2], float.Parse(row[3]), float.Parse(row[4]), float.Parse(row[5])));
        }
        return costumeDic;
    }


    /// <summary>�������� �ڽ�Ƭ ������ �޾ƿ� ��ųʸ��� �ִ� �Լ�</summary>
    private void CostumeParserByServer(BackendReturnObject callback)
    {
        CostumeDic.Clear();
        JsonData json = callback.FlattenRows();

        for (int i = 0, count = json.Count; i < count; i++)
        {
            string costumeID = json[i]["CostumeID"].ToString();
            string bodyParts = json[i]["BodyParts"].ToString();
            string costumeName = json[i]["CostumeName"].ToString();
            float posX = float.Parse(json[i]["PosX"].ToString());
            float posY = float.Parse(json[i]["PosY"].ToString());
            float posZ = float.Parse(json[i]["PosZ"].ToString());

            CostumeDic.Add(costumeID, new CostumeData(costumeID, (EBodyParts)Enum.Parse(typeof(EBodyParts), bodyParts), costumeName, posX, posY, posZ));
        }

        Debug.Log("�ڽ�Ƭ ������ �޾ƿ��� ����");
    }


    public CostumeData GetCostumeData(string costumeID) 
    {
        CostumeData costumeData = CostumeDic[costumeID];
        return costumeData;
    }
}
