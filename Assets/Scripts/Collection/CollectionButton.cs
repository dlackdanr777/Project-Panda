using UnityEngine;
using BT;
using System;

public class CollectionButton : MonoBehaviour, IInteraction
{
    public Action<float> OnCollectionButtonClicked;

    private float _fadeTime = 1f; // 화면 어두운 시간


    public void StartInteraction()
    {
        OnCollectionButtonClicked?.Invoke(_fadeTime); // 화면 FadeOut

        gameObject.SetActive(false);
    }

    public void UpdateInteraction()
    {

    }

    public void ExitInteraction()
    {

    }
}
