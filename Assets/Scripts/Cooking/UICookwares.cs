using Muks.Tween;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICookwares : MonoBehaviour
{
    [SerializeField] private Transform _parent;

    [SerializeField] private GameObject _dontTouchArea;

    [SerializeField] private UICookingFood[] _foods;

    private Image[] _cookwareImages;

    private RectTransform _parentRect;

    private int _cookwareIndex;

    private float _imageWidth;

    private float _tempAnchoredPosition_X;

    public void Init()
    {
        _parentRect = _parent.GetComponent<RectTransform>();
        _tempAnchoredPosition_X = _parentRect.anchoredPosition.x;
        _cookwareImages = _parent.GetComponentsInChildren<Image>();
        _imageWidth = _cookwareImages[0].rectTransform.rect.height;

        foreach(UICookingFood food in _foods)
        {
            food.gameObject.SetActive(false);
        }

        _dontTouchArea.gameObject.SetActive(false);
    }


    public void ChangeImage(int value, Action onComplated = null)
    {
        _dontTouchArea.gameObject.SetActive(true);
        float movePos = _imageWidth * -value;
        _cookwareIndex += value;
        Vector2 targetPos = new Vector2(movePos + _tempAnchoredPosition_X, 0);
        Tween.RectTransfromAnchoredPosition(_parent.gameObject, targetPos, 0.4f, TweenMode.Constant, () =>
        {
            Tween.RectTransfromAnchoredPosition(_parent.gameObject, targetPos, 0.1f, TweenMode.Constant, () =>
            {
                _dontTouchArea.gameObject.SetActive(false);
                _parentRect.anchoredPosition = targetPos;
                _tempAnchoredPosition_X += movePos;
                onComplated?.Invoke();
            });
        });
    }

    public void SetFoodSprite(RecipeData data, float fireValue)
    {
        _foods[_cookwareIndex].SetFoodSprite(data, fireValue);
    }

        public void StartAnime()
    {
        _foods[_cookwareIndex].StartAnime();
    }

    public void StartCooking()
    {
        _foods[_cookwareIndex].ResetSprite();
        _foods[_cookwareIndex].gameObject.SetActive(true);
    }

    public void EndCooking()
    {
        _foods[_cookwareIndex].gameObject.SetActive(false);

    }
}
