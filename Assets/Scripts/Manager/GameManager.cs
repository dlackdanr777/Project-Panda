using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonHandler<GameManager>
{
    public bool IsStart;

    public Player Player;
    public MessageDatabase MessageDatabase;

    /// <summary> ���� ��� ī�޶� �̵��� ���´�. </summary>
    public bool FriezeCameraMove;

    /// <summary> ���� ��� ī�޶� ���� ���´�. </summary>
    public bool FriezeCameraZoom;

    /// <summary> ���� ��� ��ȣ�ۿ��� ���´�. </summary>
    public bool FirezeInteraction;

    public override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        Player = new Player();
        Player.Init();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        { Player.GainBamboo(1); }

    }


    public void OnApplicationQuit()
    {
        DatabaseManager.Instance.UserInfo.SaveUserInfoData(10);
        DatabaseManager.Instance.UserInfo.SaveAttendanceData(10);
        DatabaseManager.Instance.UserInfo.SaveNPCData(10);
        Player.SaveBambooData(10);
        DatabaseManager.Instance.UserInfo.SaveInventoryData(10);
        DatabaseManager.Instance.UserInfo.SaveStickerData(10);
        DatabaseManager.Instance.FurniturePosDatabase.SaveFurnitureData(10);
        DatabaseManager.Instance.StartPandaInfo.SavePandaInfoData(10);
    }
}
