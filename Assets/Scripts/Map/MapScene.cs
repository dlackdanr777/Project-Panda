using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;
using Muks.DataBind;

public class MapScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataBind.SetTextValue("BambooCount", GameManager.Instance.Player.Bamboo.ToString());
        DatabaseManager.Instance.Challenges.CheckIsDone();
        StarterPanda.Instance.SwitchingScene();
    }
}
