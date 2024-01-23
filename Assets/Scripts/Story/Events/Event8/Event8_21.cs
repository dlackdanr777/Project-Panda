using Muks.Tween;
using System;
using UnityEngine;

public class Event8_21 : StoryEvent
{
    [SerializeField] private GameObject _starterPanda;
    [SerializeField] private GameObject _Jiji;
    [SerializeField] private GameObject _fish;
    [SerializeField] private MapButton _mapButton;
    public override void EventStart(Action onComplate)
    {
        _mapButton.MoveWishTree();

        _starterPanda.SetActive(true);
        _Jiji.SetActive(true);
        _fish.SetActive(true);

        Invoke("MoveCamera", 1.1f);
        Tween.SpriteRendererAlpha(gameObject, 1, 1, TweenMode.Quadratic, onComplate);
    }

    public override void EventCancel(Action onComplate = null)
    {
        _starterPanda.SetActive(false);
        _Jiji.SetActive(false);
        _fish.SetActive(false);

        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
    }

    private void MoveCamera()
    {
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        Camera.main.gameObject.transform.position = targetPos;
        
    }
}