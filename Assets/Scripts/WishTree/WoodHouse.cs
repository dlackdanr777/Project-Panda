using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodHouse : MonoBehaviour, IInteraction
{
    [SerializeField] private UIInsideWood _uiHouse;

    [SerializeField] private UINavigation _uiNav;

    [SerializeField] private SpriteRenderer _outsideWood;

    public void Start()
    {
        _uiHouse.OnShowHandler += () => Tween.SpriteRendererAlpha(_outsideWood.gameObject, 0, 0.2f, TweenMode.Smoothstep);
        _uiHouse.OnHideHandler += () => Tween.SpriteRendererAlpha(_outsideWood.gameObject, 1, 0.2f, TweenMode.Smoothstep);
    }

    public void StartInteraction()
    {
        _uiNav.Push("InsideWood");
    }


    public void UpdateInteraction()
    {
    }


    public void ExitInteraction()
    {
        _uiNav.Pop("InsideWood");
    }
}
