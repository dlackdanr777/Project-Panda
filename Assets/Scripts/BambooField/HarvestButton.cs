using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestButton : MonoBehaviour, IInteraction
{
    //public FieldSlot FieldSlot;
    public BambooFieldSystem BambooFieldSystem;
    public bool IsSet;

    public void StartInteraction()
    {
        if (IsSet)
        {
            Debug.Log("작물 수확 버튼 클릭");
            BambooFieldSystem.ClickHavestButton();
        }
    }
    public void UpdateInteraction()
    {

    }
    public void ExitInteraction()
    {

    }

}
