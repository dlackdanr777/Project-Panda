using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISuccessLocation : MonoBehaviour
{

    [SerializeField] private Image _successRange_S;

    [SerializeField] private Image _successRange_A;

    [SerializeField] private Image _successRange_B;

    private RectTransform _rectTransform;

    public void Init()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetSuccessRange(RecipeData recipe, float gaugeWidth)
    {
        float gaugePos = gaugeWidth * recipe.SuccessLocation;
        _rectTransform.anchoredPosition = new Vector2(gaugePos, _rectTransform.anchoredPosition.y);

        float successRangeS_x = gaugeWidth * recipe.SuccessRangeLevel_S;
        float successRangeA_x = successRangeS_x + (gaugeWidth * recipe.SuccessRangeLevel_A);
        float successRangeB_x = successRangeA_x + (gaugeWidth * recipe.SuccessRangeLevel_B);

        _successRange_S.rectTransform.sizeDelta = new Vector2(successRangeS_x, _successRange_S.rectTransform.sizeDelta.y);
        _successRange_A.rectTransform.sizeDelta = new Vector2(successRangeA_x, _successRange_A.rectTransform.sizeDelta.y);
        _successRange_B.rectTransform.sizeDelta = new Vector2(successRangeB_x, _successRange_B.rectTransform.sizeDelta.y);
    }
}
