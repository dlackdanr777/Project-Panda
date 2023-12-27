using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;

public class UICollection : MonoBehaviour
{
    [SerializeField] private Image _fadeInOut;
    [SerializeField] private Image _resultTextImage;
    [SerializeField] private TextMeshProUGUI _resultText;

    [SerializeField] private CollectionButton _collectionButton;

    void Start()
    {
        _collectionButton.OnCollectionButtonClicked += FadeInOut;
    }


    private void FadeInOut(float fadeTime)
    {
        _fadeInOut.gameObject.SetActive(true);
        Tween.IamgeAlpha(_fadeInOut.gameObject, 1, fadeTime, TweenMode.Quadratic, () =>
        {
            Tween.IamgeAlpha(_fadeInOut.gameObject, 0, fadeTime, TweenMode.Quadratic, () =>
            {
                _fadeInOut.gameObject.SetActive(false);
            });
        });
    }

    private void DesplaySesultText()
    {

    }
}
