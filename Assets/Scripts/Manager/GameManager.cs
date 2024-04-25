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

    /// <summary>ù ���ξ� ���� Ȯ��</summary>
    public bool IsFirstAccessMainScene; //�⼮ üũ UI�� ù ���ӿ��� ���̰� �Ҷ� ���


    public override void Awake()
    {
        Application.targetFrameRate = 60;
        base.Awake();
        _option = new PreferencesData();
        _tmpCameraData = new TempCameraData();
        Player = new Player();
        Player.Init();
    }


    public void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            DatabaseManager.Instance.UserInfo.SaveUserInfoData(3);
            DatabaseManager.Instance.UserInfo.ChallengesUserData.SaveChallengesData(3);
            BambooFieldSystem.Instance.SaveBambooFieldData(3);
            Player.SaveBambooData(3);
        }
    }


    public void OnApplicationQuit()
    {
        DatabaseManager.Instance.UserInfo.SaveUserInfoData(10);
        DatabaseManager.Instance.UserInfo.ChallengesUserData.SaveChallengesData(10);
        DatabaseManager.Instance.UserInfo.StoryUserData.SaveStoryData(10);
        DatabaseManager.Instance.UserInfo.AttendanceUserData.SaveAttendanceData(10);
        DatabaseManager.Instance.UserInfo.NpcUserData.SaveNPCData(10);
        Player.SaveBambooData(10);
        DatabaseManager.Instance.UserInfo.MailUserData.SaveMailData(10);
        DatabaseManager.Instance.UserInfo.InventoryUserData.SaveInventoryData(10);
        DatabaseManager.Instance.UserInfo.BookUserData.SaveBookData(10);
        DatabaseManager.Instance.FurniturePosDatabase.SaveFurnitureData(10);
        DatabaseManager.Instance.StartPandaInfo.SavePandaInfoData(10);
        BambooFieldSystem.Instance.SaveBambooFieldData(10);
    }
}
