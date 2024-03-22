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

    private PreferencesData _option;
    public PreferencesData Option => _option;

    private TempCameraData _tmpCameraData;
    public TempCameraData TmpCameraData => _tmpCameraData;

    /// <summary> ���� ��� ī�޶� �̵��� ���´�. </summary>
    public bool FriezeCameraMove;

    /// <summary> ���� ��� ī�޶� ���� ���´�. </summary>
    public bool FriezeCameraZoom;

    /// <summary> ���� ��� ��ȣ�ۿ��� ���´�. </summary>
    public bool FirezeInteraction;

    public override void Awake()
    {
        base.Awake();
        _option = new PreferencesData();
        _tmpCameraData = new TempCameraData();
        Application.targetFrameRate = 60;
        Player = new Player();
        Player.Init();
    }


    public void OnApplicationQuit()
    {
        DatabaseManager.Instance.UserInfo.SaveUserInfoData(10);
        DatabaseManager.Instance.UserInfo.SaveChallengesData(10);
        DatabaseManager.Instance.UserInfo.SaveStoryData(10);
        DatabaseManager.Instance.UserInfo.SaveAttendanceData(10);
        DatabaseManager.Instance.UserInfo.SaveNPCData(10);
        Player.SaveBambooData(10);
        Player.SaveMailData(10);
        DatabaseManager.Instance.UserInfo.SaveInventoryData(10);
        DatabaseManager.Instance.UserInfo.SaveStickerData(10);
        DatabaseManager.Instance.FurniturePosDatabase.SaveFurnitureData(10);
        DatabaseManager.Instance.StartPandaInfo.SavePandaInfoData(10);
    }
}
