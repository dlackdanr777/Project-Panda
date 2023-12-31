using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonHandler<GameManager>
{
    public bool IsStart;

    public Player Player;
    public MessageDatabase MessageDatabase;

    /// <summary> 참일 경우 카메라 이동을 막는다. </summary>
    public bool FriezeCameraMove;

    /// <summary> 참일 경우 카메라 줌을 막는다. </summary>
    public bool FriezeCameraZoom;

    /// <summary> 참일 경우 상호작용을 막는다. </summary>
    public bool FirezeInteraction;

    public override void Awake()
    {
        base.Awake();
    }

}
