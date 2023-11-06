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
            //���� ������Ʈ�� TweenMove ��ũ��Ʈ�� �����ϸ� �������� ���� ��� ��ũ��Ʈ�� �߰��Ͽ� �����´�. 
            TweenMove objToMove = !targetObject.GetComponent<TweenMove>()
                ? targetObject.AddComponent<TweenMove>()
                : targetObject.GetComponent<TweenMove>();

            objToMove.ElapsedDuration = 0;
            objToMove.StartPosition = targetObject.transform.position;
            objToMove.TargetPosition = targetPosition;
            objToMove.TotalDuration = duration;
            objToMove.TweenMode = tweenMode;
            objToMove.OnComplete = onComplete;
            objToMove.enabled = true;
        }

        public static void Rotate(GameObject targetObject, Vector3 targetEulerAngles, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null)
        {
            //���� ������Ʈ�� TweenRotate ��ũ��Ʈ�� �����ϸ� �������� ���� ��� ��ũ��Ʈ�� �߰��Ͽ� �����´�. 
            TweenRotate objToRotate = !targetObject.GetComponent<TweenRotate>()
                ? targetObject.AddComponent<TweenRotate>()
                : targetObject.GetComponent<TweenRotate>();

            objToRotate.ElapsedDuration = 0;
            objToRotate.StartEulerAngles = targetObject.transform.eulerAngles;
            objToRotate.TargetEulerAngles = targetEulerAngles;
            objToRotate.TotalDuration = duration;
            objToRotate.TweenMode = tweenMode;
            objToRotate.OnComplete = onComplete;
            objToRotate.enabled = true;
        }

        public static void Scale(GameObject targetObject, Vector3 targetScale, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null)
        {
            //���� ������Ʈ�� TweenScale ��ũ��Ʈ�� �����ϸ� �������� ���� ��� ��ũ��Ʈ�� �߰��Ͽ� �����´�. 
            TweenScale objToScale = !targetObject.GetComponent<TweenScale>()
                ? targetObject.AddComponent<TweenScale>()
                : targetObject.GetComponent<TweenScale>();

            objToScale.ElapsedDuration = 0;
            objToScale.StartScale = targetObject.transform.localScale;
            objToScale.TargetScale = targetScale;
            objToScale.TotalDuration = duration;
            objToScale.TweenMode = tweenMode;
            objToScale.OnComplete = onComplete;
            objToScale.enabled = true;
        }

        public static void SizeDelta(RectTransform rectTransform, Vector2 targetSizeDelta, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null)
        {
            //���� ������Ʈ�� TweenSizeDelta ��ũ��Ʈ�� �����ϸ� �������� ���� ��� ��ũ��Ʈ�� �߰��Ͽ� �����´�. 
            TweenSizeDelta objToSizeDelta = !rectTransform.gameObject.GetComponent<TweenSizeDelta>()
                ? rectTransform.gameObject.AddComponent<TweenSizeDelta>()
                : rectTransform.gameObject.GetComponent<TweenSizeDelta>();

            objToSizeDelta.ElapsedDuration = 0;
            objToSizeDelta.RectTransform = rectTransform;
            objToSizeDelta.StartSizeDelta = rectTransform.sizeDelta;
            objToSizeDelta.TargetSizeDelta = targetSizeDelta;
            objToSizeDelta.TotalDuration = duration;
            objToSizeDelta.TweenMode = tweenMode;
            objToSizeDelta.OnComplete = onComplete;
            objToSizeDelta.enabled = true;
        }

        public static void Color(Image targetImage, Color targetColor, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null)
        {
            //���� ������Ʈ�� TweenColor ��ũ��Ʈ�� �����ϸ� �������� ���� ��� ��ũ��Ʈ�� �߰��Ͽ� �����´�. 
            TweenColor objToColor = !targetImage.GetComponent<TweenColor>()
                ? targetImage.AddComponent<TweenColor>()
                : targetImage.GetComponent<TweenColor>();

            objToColor.ElapsedDuration = 0;
            objToColor.Image = targetImage;
            objToColor.StartColor = targetImage.color;
            objToColor.TargetColor = targetColor;
            objToColor.TotalDuration = duration;
            objToColor.TweenMode = tweenMode;
            objToColor.OnComplete = onComplete;
            objToColor.enabled = true;
        }
    }
}

