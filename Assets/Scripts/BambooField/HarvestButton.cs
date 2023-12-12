using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestButton : MonoBehaviour, IInteraction
{
    public FieldSlot FieldSlot;
    public void StartInteraction()
    {
        FieldSlot.ClickHavestButton();
    }
    public void UpdateInteraction()
    {

    }
    public void ExitInteraction()
    {
        gameObject.SetActive(false);
    }

}
