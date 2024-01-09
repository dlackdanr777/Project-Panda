using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

}
