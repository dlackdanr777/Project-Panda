using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

///<summary> ����ð��� ���� �ӵ��� ��� �޸� ���� ���ΰ�? </summary>
public enum TweenMode
{
    /// <summary>���</summary>
    Constant,

    /// <summary>����</summary>
    Quadratic,

    /// <summary>õõ�� ���� õõ�� ����</summary>
    Smoothstep,

    /// <summary>���� õõ�� ���� ���� õõ�� ����</summary>
    Smootherstep,

    /// <summary>������ ��ġ�� ���ٰ� ���ڸ��� ���ư�</summary>
    Spike,

    /// <summary>������ ��ġ�� ���� ������ ƨ��</summary>
    EaseInOutElastic,

    /// <summary>�������ϰ� ��ġ�� ���� �ѹ� ƨ��</summary>
    EaseInOutBack,

    /// <summary>Sin �׷��� �̵�</summary>
    Sinerp,

    /// <summary>Cos �׷��� �̵�</summary>
    Coserp,
}

/// <summary>
/// Ʈ�� �ִϸ��̼��� ���� ���� Ŭ����
/// </summary>

namespace Muks.Tween
{
    public static class Tween
    {
        /// <summary>
        /// ���ӽð���ŭ ������Ʈ�� �̵���Ű�� �Լ�
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

