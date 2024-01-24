using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;
using Unity.VisualScripting;

public class MapScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StarterPanda.Instance.SwitchingScene();
    }
}
