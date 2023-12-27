using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingHandler : MonoBehaviour, IInteraction
{
    [SerializeField] private Sprite _exclamationMark;
    private void EndAnimation()
    {
        // 애니메이션 완료되면 느낌표 표시
        GetComponent<SpriteRenderer>().sprite = _exclamationMark;
    }

    // 느낌표 표시한 후 누르면 성공인지 실패인지 표시
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
