using UnityEngine;
using System;
using Newtonsoft.Json.Linq;
using Muks.Tween;

public class CollectingHandler : MonoBehaviour
{
    public Action<bool> OnExclamationMarkClicked;

    [SerializeField] private GameObject _checkResultButton;

    private void EndAnimation()
    {
        GetComponent<Animator>().enabled = false;

        // 애니메이션 완료되면 느낌표 표시
        _checkResultButton.SetActive(true);

        gameObject.SetActive(false);
    }
    private void MoveUp()
    {
        Tween.TransformMove(gameObject, gameObject.transform.position + Vector3.up * 3.5f, 0.5f, TweenMode.Constant);
    }

    private void MoveDown()
    {
        Tween.TransformMove(gameObject, gameObject.transform.position - Vector3.up * 3.5f, 0.5f, TweenMode.Constant);
    }
}
