using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewSystem : MonoBehaviour
{
    [SerializeField] private ScrollButton _leftButton;
    [SerializeField] private ScrollButton _rightButton;
    [SerializeField] private float _scrollSpeed = 0.01f;

    private ScrollRect _scrollRect;

    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    private void Update()
    {
        if(_leftButton != null)
        {
            if (_leftButton.isDown)
            {
                ScrollLeft();
            }
        }
        if(_rightButton != null)
        {
            if(_rightButton.isDown)
            {
                ScrollRight();
            }
        }
    }

    private void ScrollLeft()
    {
        if(_scrollRect != null)
        {
            if(_scrollRect.horizontalNormalizedPosition >= 0f)
            {
                _scrollRect.horizontalNormalizedPosition -= _scrollSpeed;
            }
        }
    }

    private void ScrollRight() 
    {
        if(_scrollRect != null)
        {
            if(_scrollRect.horizontalNormalizedPosition <= 1f)
            {
                _scrollRect.horizontalNormalizedPosition += _scrollSpeed;
            }
        }
    }
}
