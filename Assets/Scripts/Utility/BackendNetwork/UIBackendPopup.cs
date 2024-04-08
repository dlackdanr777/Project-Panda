using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[RequireComponent(typeof(CanvasGroup))]
public class UIBackendPopup : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI _errorNameText;
    [SerializeField] private TextMeshProUGUI _errorDescriptionText;
    [SerializeField] private Button _okButton;


    [Header("Animation Options")]
    [SerializeField] private GameObject _target;


    private CanvasGroup _canvasGroup;

    private Vector3 _startScale => new Vector3(0.8f, 0.8f, 0.8f);
    private Vector3 _targetScale => new Vector3(1,1,1);

    public void Init()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(false);
    }


    public void Show(string errorName, string errorDescription, UnityAction onButtonClicked = null)
    {
        gameObject.SetActive(true);
        _canvasGroup.blocksRaycasts = false;

        _errorNameText.text = errorName;
        _errorDescriptionText.text = errorDescription;

        _okButton.onClick.RemoveAllListeners();
        _okButton.onClick.AddListener(Hide);

        if (onButtonClicked != null)
            _okButton.onClick.AddListener(onButtonClicked);

        _target.transform.localScale = _startScale;
        Tween.TransformScale(_target, _targetScale, 0.3f, TweenMode.EaseOutBack, () =>
        {
            _canvasGroup.blocksRaycasts = true;
        });

    }


    private void Hide()
    {
        _canvasGroup.blocksRaycasts = false;

        _target.transform.localScale = _targetScale;
        Tween.TransformScale(_target, _startScale * 0.5f, 0.3f, TweenMode.EaseInBack, () =>
        {
            gameObject.SetActive(false);
        });
    }
}
