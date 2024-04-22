using UnityEngine;
using System;
using Newtonsoft.Json.Linq;
using Muks.Tween;
using BT;

public class CollectingHandler : MonoBehaviour
{
    public Action<bool> OnExclamationMarkClicked;

    [SerializeField] private GameObject _checkResultButton;
    [SerializeField] private GatheringItemType _gatheringItemType;
    private void EndAnimation()
    {
        GetComponent<Animator>().enabled = false;

        // �ִϸ��̼� �Ϸ�Ǹ� ����ǥ ǥ��
        _checkResultButton.SetActive(true);

        gameObject.SetActive(false);
    }
    private void MoveUp()
    {
        if (_gatheringItemType == GatheringItemType.Bug){
            Tween.TransformMove(gameObject, gameObject.transform.position + Vector3.up * 3.5f, 0.5f, TweenMode.Constant);
        }
    }

    private void MoveDown()
    {
        if (_gatheringItemType == GatheringItemType.Bug){
            Tween.TransformMove(gameObject, gameObject.transform.position - Vector3.up * 3.5f, 0.5f, TweenMode.Constant);
        }
    }
}
