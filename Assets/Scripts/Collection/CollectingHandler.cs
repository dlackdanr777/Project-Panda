using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingHandler : MonoBehaviour, IInteraction
{
    [SerializeField] private Sprite _exclamationMark;
    private void EndAnimation()
    {
        // �ִϸ��̼� �Ϸ�Ǹ� ����ǥ ǥ��
        GetComponent<SpriteRenderer>().sprite = _exclamationMark;
    }

    // ����ǥ ǥ���� �� ������ �������� �������� ǥ��
    public void StartInteraction()
    {

    }

    public void UpdateInteraction()
    {

    }

    public void ExitInteraction()
    {

    }
}
