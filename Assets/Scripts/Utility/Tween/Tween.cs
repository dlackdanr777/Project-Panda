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

    /// <summary>빠르게 위치로 가서 한번 튕김</summary>
    Back,

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
            //만약 오브젝트에 TweenMove 스크립트가 존재하면 가져오고 없을 경우 스크립트를 추가하여 가져온다. 
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
            //만약 오브젝트에 TweenRotate 스크립트가 존재하면 가져오고 없을 경우 스크립트를 추가하여 가져온다. 
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
            //만약 오브젝트에 TweenScale 스크립트가 존재하면 가져오고 없을 경우 스크립트를 추가하여 가져온다. 
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
            //만약 오브젝트에 TweenSizeDelta 스크립트가 존재하면 가져오고 없을 경우 스크립트를 추가하여 가져온다. 
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
            //만약 오브젝트에 TweenColor 스크립트가 존재하면 가져오고 없을 경우 스크립트를 추가하여 가져온다. 
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

