using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonHandler<GameManager>
{
    public bool IsStart;
    public bool IsFirstStart;

    //public Player1 Player1;
    public Player Player;
    //public UIMessage1 UIMessage; //삭제 예정
    public MessageDatabase MessageDatabase;

}
