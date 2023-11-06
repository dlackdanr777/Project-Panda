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

    /// <summary>������ ��ġ�� ���� �ѹ� ƨ��</summary>
    Back,

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

            if(objToMove.enabled)
            {
                DataSequence tempData = new DataSequence();
                tempData.StartObject = targetObject.transform.position;
                tempData.TargetObject = targetPosition;
                tempData.Duration = duration;
                tempData.TweenMode = tweenMode;
                tempData.OnComplete = onComplete;
                objToMove.SetDataSequence(tempData);
            }

            else
            {
                objToMove.ElapsedDuration = 0;
                objToMove.StartPosition = targetObject.transform.position;
                objToMove.TargetPosition = targetPosition;
                objToMove.TotalDuration = duration;
                objToMove.TweenMode = tweenMode;
                objToMove.OnComplete = onComplete;
                objToMove.enabled = true;
            }

        }

        public static void Rotate(GameObject targetObject, Vector3 targetEulerAngles, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null)
        {
            //���� ������Ʈ�� TweenRotate ��ũ��Ʈ�� �����ϸ� �������� ���� ��� ��ũ��Ʈ�� �߰��Ͽ� �����´�. 
            TweenRotate objToRotate = !targetObject.GetComponent<TweenRotate>()
                ? targetObject.AddComponent<TweenRotate>()
                : targetObject.GetComponent<TweenRotate>();

            if (objToRotate.enabled)
            {
                DataSequence tempData = new DataSequence();
                tempData.StartObject = targetObject.transform.rotation;
                tempData.TargetObject = targetEulerAngles;
                tempData.Duration = duration;
                tempData.TweenMode = tweenMode;
                tempData.OnComplete = onComplete;
                objToRotate.SetDataSequence(tempData);
            }
            else
            {
                objToRotate.ElapsedDuration = 0;
                objToRotate.StartEulerAngles = targetObject.transform.eulerAngles;
                objToRotate.TargetEulerAngles = targetEulerAngles;
                objToRotate.TotalDuration = duration;
                objToRotate.TweenMode = tweenMode;
                objToRotate.OnComplete = onComplete;
                objToRotate.enabled = true;
            }

            
        }

        public static void Scale(GameObject targetObject, Vector3 targetScale, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null)
        {
            //���� ������Ʈ�� TweenScale ��ũ��Ʈ�� �����ϸ� �������� ���� ��� ��ũ��Ʈ�� �߰��Ͽ� �����´�. 
            TweenScale objToScale = !targetObject.GetComponent<TweenScale>()
                ? targetObject.AddComponent<TweenScale>()
                : targetObject.GetComponent<TweenScale>();

            if (objToScale.enabled)
            {
                DataSequence tempData = new DataSequence();
                tempData.StartObject = targetObject.transform.localScale;
                tempData.TargetObject = targetScale;
                tempData.Duration = duration;
                tempData.TweenMode = tweenMode;
                tempData.OnComplete = onComplete;
                objToScale.SetDataSequence(tempData);
            }
            else
            {
                objToScale.ElapsedDuration = 0;
                objToScale.StartScale = targetObject.transform.localScale;
                objToScale.TargetScale = targetScale;
                objToScale.TotalDuration = duration;
                objToScale.TweenMode = tweenMode;
                objToScale.OnComplete = onComplete;
                objToScale.enabled = true;
            }

        }

        public static void SizeDelta(RectTransform rectTransform, Vector2 targetSizeDelta, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null)
        {
            //���� ������Ʈ�� TweenSizeDelta ��ũ��Ʈ�� �����ϸ� �������� ���� ��� ��ũ��Ʈ�� �߰��Ͽ� �����´�. 
            TweenSizeDelta objToSizeDelta = !rectTransform.gameObject.GetComponent<TweenSizeDelta>()
                ? rectTransform.gameObject.AddComponent<TweenSizeDelta>()
                : rectTransform.gameObject.GetComponent<TweenSizeDelta>();

            if (objToSizeDelta.enabled)
            {
                DataSequence tempData = new DataSequence();
                tempData.Object = rectTransform;
                tempData.StartObject = rectTransform.sizeDelta;
                tempData.TargetObject = targetSizeDelta;
                tempData.Duration = duration;
                tempData.TweenMode = tweenMode;
                tempData.OnComplete = onComplete;
                objToSizeDelta.SetDataSequence(tempData);
            }
            else
            {
                objToSizeDelta.ElapsedDuration = 0;
                objToSizeDelta.RectTransform = rectTransform;
                objToSizeDelta.StartSizeDelta = rectTransform.sizeDelta;
                objToSizeDelta.TargetSizeDelta = targetSizeDelta;
                objToSizeDelta.TotalDuration = duration;
                objToSizeDelta.TweenMode = tweenMode;
                objToSizeDelta.OnComplete = onComplete;
                objToSizeDelta.enabled = true;
            }   
        }

        public static void Color(Image targetImage, Color targetColor, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null)
        {
            //���� ������Ʈ�� TweenColor ��ũ��Ʈ�� �����ϸ� �������� ���� ��� ��ũ��Ʈ�� �߰��Ͽ� �����´�. 
            TweenColor objToColor = !targetImage.GetComponent<TweenColor>()
                ? targetImage.AddComponent<TweenColor>()
                : targetImage.GetComponent<TweenColor>();

            if (objToColor.enabled)
            {
                DataSequence tempData = new DataSequence();
                tempData.Object = targetImage;
                tempData.StartObject = targetImage.color;
                tempData.TargetObject = targetColor;
                tempData.Duration = duration;
                tempData.TweenMode = tweenMode;
                tempData.OnComplete = onComplete;
                objToColor.SetDataSequence(tempData);
            }
            else
            {
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
}

