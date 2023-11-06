using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

///<summary> 경과시간에 따라 속도를 어떻게 달리 해줄 것인가? </summary>
public enum TweenMode
{
    /// <summary>등속</summary>
    Constant,

    /// <summary>가속</summary>
    Quadratic,

    /// <summary>천천히 가속 천천히 감속</summary>
    Smoothstep,

    /// <summary>더욱 천천히 가속 더욱 천천히 감속</summary>
    Smootherstep,

    /// <summary>빠르게 위치로 갔다가 제자리로 돌아감</summary>
    Spike,

    /// <summary>빠르게 위치로 가서 여러번 튕김</summary>
    EaseInOutElastic,

    /// <summary>스무스하게 위치로 가서 한번 튕김</summary>
    EaseInOutBack,

    /// <summary>Sin 그래프 이동</summary>
    Sinerp,

    /// <summary>Cos 그래프 이동</summary>
    Coserp,
}

/// <summary>
/// 트윈 애니메이션을 위한 정적 클래스
/// </summary>

namespace Muks.Tween
{
    public static class Tween
    {
        /// <summary>
        /// 지속시간만큼 오브젝트를 이동시키는 함수
        /// </summary>
        public static void Move(GameObject targetObject, Vector3 targetPosition, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null)
        {
            TweenMove objToMove = !targetObject.GetComponent<TweenMove>()
                ? targetObject.AddComponent<TweenMove>()
                : targetObject.GetComponent<TweenMove>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetPosition;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            objToMove.SetDataSequence(tempData);

            if (!objToMove.enabled)
            {
                objToMove.ElapsedDuration = 0;
                objToMove.TotalDuration = 0;
                objToMove.enabled = true;
            }
        }


        public static void Rotate(GameObject targetObject, Vector3 targetEulerAngles, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null)
        {
            TweenRotate objToRotate = !targetObject.GetComponent<TweenRotate>()
                ? targetObject.AddComponent<TweenRotate>()
                : targetObject.GetComponent<TweenRotate>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetEulerAngles;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            objToRotate.SetDataSequence(tempData);

            if (!objToRotate.enabled)
            {
                objToRotate.ElapsedDuration = 0;
                objToRotate.TotalDuration = 0;
                objToRotate.enabled = true;
            }
        }


        public static void Scale(GameObject targetObject, Vector3 targetScale, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null)
        {
            TweenScale objToScale = !targetObject.GetComponent<TweenScale>()
                ? targetObject.AddComponent<TweenScale>()
                : targetObject.GetComponent<TweenScale>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetScale;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            objToScale.SetDataSequence(tempData);

            if (!objToScale.enabled)
            {
                objToScale.ElapsedDuration = 0;
                objToScale.TotalDuration = 0;
                objToScale.enabled = true;
            }
        }


        public static void SizeDelta(GameObject targetObject, Vector2 targetSizeDelta, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null)
        {
            TweenSizeDelta objToSizeDelta = !targetObject.GetComponent<TweenSizeDelta>()
                ? targetObject.AddComponent<TweenSizeDelta>()
                : targetObject.GetComponent<TweenSizeDelta>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetSizeDelta;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            objToSizeDelta.SetDataSequence(tempData);

            if (!objToSizeDelta.enabled)
            {
                objToSizeDelta.ElapsedDuration = 0;
                objToSizeDelta.TotalDuration = 0;
                objToSizeDelta.enabled = true;
            }
        }


        public static void Color(GameObject targetObject, Color targetColor, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null)
        {
            TweenColor objToColor = !targetObject.GetComponent<TweenColor>()
                ? targetObject.AddComponent<TweenColor>()
                : targetObject.GetComponent<TweenColor>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetColor;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            objToColor.SetDataSequence(tempData);

            if (!objToColor.enabled)
            {
                objToColor.ElapsedDuration = 0;
                objToColor.TotalDuration = 0;
                objToColor.enabled = true;
            }
        }
    }
}

