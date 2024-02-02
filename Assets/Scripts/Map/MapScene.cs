using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;
using Unity.VisualScripting;
using Muks.DataBind;

public class MapScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataBind.SetTextValue("BambooCount", GameManager.Instance.Player.Bamboo.ToString());
        StarterPanda.Instance.SwitchingScene();
    }
}
