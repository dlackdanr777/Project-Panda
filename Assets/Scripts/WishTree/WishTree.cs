using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishTree : MonoBehaviour
{
    [SerializeField] private UIInsideWood _uiWishTree;

    private bool _isEnabled;


    public void Start()
    {
        _uiWishTree.OnShowHandler += EntranceWishTree;
        _uiWishTree.OnHideHandler += ExitWishTree;

    }

    private void Update()
    {
        if (!_isEnabled)
            return;

        
    }


    private void EntranceWishTree()
    {
        _isEnabled = true;
    }


    private void ExitWishTree()
    {
        _isEnabled = false;
    }
}
