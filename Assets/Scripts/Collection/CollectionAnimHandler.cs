using UnityEngine;
using Muks.Tween;
using BT;

public class CollectionAnimHandler : MonoBehaviour
{
    [SerializeField] private GameObject _splashObject;

    private void SplashStart()
    {
        _splashObject.SetActive(true);
    }

    private void SplashEnd()
    {
        _splashObject.SetActive(false);
    }

    private void MoveUp()
    {
        Tween.TransformMove(gameObject, gameObject.transform.position + Vector3.up * 3, 0.5f, TweenMode.Constant);
    }

    private void MoveDown()
    {
        Tween.TransformMove(gameObject, gameObject.transform.position - Vector3.up * 3, 0.5f, TweenMode.Constant);
    }

}
