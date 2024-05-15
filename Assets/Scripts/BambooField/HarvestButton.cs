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
            SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
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
