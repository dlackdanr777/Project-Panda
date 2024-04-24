using UnityEngine;
using BT;
using System;

public class CollectionButton : MonoBehaviour, IInteraction
{
    public Action<float> OnCollectionButtonClicked;

    private float _fadeTime = 1f; // ȭ�� ��ο� �ð�


    public void StartInteraction()
    {
        OnCollectionButtonClicked?.Invoke(_fadeTime); // ȭ�� FadeOut

        gameObject.SetActive(false);
    }

    public void UpdateInteraction()
    {

    }

    public void ExitInteraction()
    {

    }
}
