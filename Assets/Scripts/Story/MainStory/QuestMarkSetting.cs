using Muks.DataBind;
using System;
using UnityEngine;

public class QuestMarkSetting : MonoBehaviour
{
    public event Action OnEnableQuestMarkHandler;
    public event Action OnDisableQuestMarkHandler;

    private void OnEnable()
    {
        OnEnableQuestMarkHandler?.Invoke();
    }

    private void OnDisable()
    {
        OnDisableQuestMarkHandler?.Invoke();
    }
}
