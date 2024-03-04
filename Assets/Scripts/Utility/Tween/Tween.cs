using System;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>마지막쯤에 빠르게 이동</summary>
    EaseInQuint,

    /// <summary>초반에 빠르게 이동</summary>
    EaseOutQuint,

    /// <summary>중간에 빠르게 이동</summary>
    EaseInOutQuint,

    /// <summary>마지막쯤에 매우 빠르게 이동</summary>
    EaseInExpo,

    /// <summary>초반에 매우 빠르게 이동</summary>
    EaseOutExpo,

    /// <summary>중간에 매우 빠르게 이동</summary>
    EaseInOutExpo,

    /// <summary>마지막에 두번 튕기고 위치로 이동</summary>
    EaseInElastic,

    /// <summary>빠르게 위치로 가서 여러번 튕김</summary>
    EaseOutElastic,

    /// <summary>중간쯤 위치로 가서 여러번 튕김</summary>
    EaseInOutElastic,

    /// <summary>한번 뒤로 갔다가 스무스 하게 해당 값으로 이동</summary>
    EaseInBack,

    /// <summary>위치로 가서 한번 튕긴 후 해당 값으로 이동</summary>
    EaseOutBack,

    /// <summary>스무스하게 위치로 가서 한번 튕김</summary>
    EaseInOutBack,

    /// <summary>현재 값에서 여러번 튕기다 해당 값으로 이동</summary>
    EaseInBounce,

    /// <summary>목표 값 이동 후 목표 값에서 여러번 튕김</summary>
    EaseOutBounce,

    /// <summary>현재 값에서 여러번 튕긴 후 중간에 목표 값 이동 후 목표 값에서도 여러번 튕김</summary>
    EaseInOutBounce,

    /// <summary>Sin 그래프 이동</summary>
    Sinerp,

    /// <summary>Cos 그래프 이동</summary>
    Coserp,
}



namespace Muks.Tween
{

    /// <summary>트윈 애니메이션을 위한 정적 클래스</summary>
    public static class Tween
    {

        /// <summary>해당 오브젝트의 일시정지된 모든 Tween 실행 함수</summary>
        public static void Play(GameObject gameObject)
        {
            TweenData[] tweens = gameObject.GetComponents<TweenData>();

            foreach (TweenData tween in tweens)
            {
                tween.enabled = true;
            }
        }


        /// <summary>해당 오브젝트의 모든 Tween 정지 함수</summary>
        public static void Stop(GameObject gameObject)
        {
            TweenData[] tweens = gameObject.GetComponents<TweenData>();

            foreach(TweenData tween in tweens)
            {
                tween.enabled = false;
                tween.IsLoop = false;
                tween.OnComplete = null;
                tween.OnUpdate = null;
                tween.DataSequences.Clear();
            }
        }


        /// <summary>해당 오브젝트의 모든 Tween 일시 정지 함수(Play() 호출 전까지 정지)</summary>
        public static void Pause(GameObject gameObject)
        {
            TweenData[] tweens = gameObject.GetComponents<TweenData>();

            foreach (TweenData tween in tweens)
            {
                tween.enabled = false;
            }
        }


        /// <summary>목표 값으로 지속 시간동안 오브젝트를 이동시키는 함수</summary>
        public static TweenData TransformMove(GameObject targetObject, Vector3 targetPosition, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null, Action onUpdate = null)
        {
            TweenTransformMove objToMove = !targetObject.GetComponent<TweenTransformMove>()
                ? targetObject.AddComponent<TweenTransformMove>()
                : targetObject.GetComponent<TweenTransformMove>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetPosition;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            tempData.OnUpdate = onUpdate;

            objToMove.IsLoop = false;
            objToMove.AddDataSequence(tempData);

            if (!objToMove.enabled)
            {
                objToMove.ElapsedDuration = 0;
                objToMove.TotalDuration = 0;
                objToMove.enabled = true;
            }

            return objToMove;
        }


        /// <summary>목표 값으로 지속 시간동안 오브젝트를 회전시키는 함수</summary>
        public static TweenData TransformRotate(GameObject targetObject, Vector3 targetEulerAngles, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null, Action onUpdate = null)
        {
            TweenTransformRotate objToRotate = !targetObject.GetComponent<TweenTransformRotate>()
                ? targetObject.AddComponent<TweenTransformRotate>()
                : targetObject.GetComponent<TweenTransformRotate>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetEulerAngles;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            tempData.OnUpdate = onUpdate;

            objToRotate.IsLoop = false;
            objToRotate.AddDataSequence(tempData);

            if (!objToRotate.enabled)
            {
                objToRotate.ElapsedDuration = 0;
                objToRotate.TotalDuration = 0;
                objToRotate.enabled = true;
            }

            return objToRotate;
        }


        /// <summary>목표 값으로 지속 시간동안 오브젝트의 크기를 조절하는 함수</summary>
        public static TweenData TransformScale(GameObject targetObject, Vector3 targetScale, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null, Action onUpdate = null)
        {
            TweenTransformScale objToScale = !targetObject.GetComponent<TweenTransformScale>()
                ? targetObject.AddComponent<TweenTransformScale>()
                : targetObject.GetComponent<TweenTransformScale>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetScale;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            tempData.OnUpdate = onUpdate;

            objToScale.IsLoop = false;
            objToScale.AddDataSequence(tempData);

            if (!objToScale.enabled)
            {
                objToScale.ElapsedDuration = 0;
                objToScale.TotalDuration = 0;
                objToScale.enabled = true;
            }

            return objToScale;
        }


        /// <summary>목표 값으로 지속 시간동안 UI의 크기를 조절하는 함수</summary>
        public static TweenData RectTransfromSizeDelta(GameObject targetObject, Vector2 targetSizeDelta, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null, Action onUpdate = null)
        {
            TweenRectTransformSizeDelta objToSizeDelta = !targetObject.GetComponent<TweenRectTransformSizeDelta>()
                ? targetObject.AddComponent<TweenRectTransformSizeDelta>()
                : targetObject.GetComponent<TweenRectTransformSizeDelta>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetSizeDelta;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            tempData.OnUpdate = onUpdate;

            objToSizeDelta.IsLoop = false;
            objToSizeDelta.AddDataSequence(tempData);

            if (!objToSizeDelta.enabled)
            {
                objToSizeDelta.ElapsedDuration = 0;
                objToSizeDelta.TotalDuration = 0;
                objToSizeDelta.enabled = true;
            }

            return objToSizeDelta;
        }


        /// <summary>목표 값으로 지속 시간동안 UI의 위치를 이동시키는 함수</summary>
        public static TweenData RectTransfromAnchoredPosition(GameObject targetObject, Vector2 targetAnchoredPosition, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null, Action onUpdate = null)
        {
            TweenRectTransformAnchoredPosition objToAnchoredPosition = !targetObject.GetComponent<TweenRectTransformAnchoredPosition>()
                ? targetObject.AddComponent<TweenRectTransformAnchoredPosition>()
                : targetObject.GetComponent<TweenRectTransformAnchoredPosition>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetAnchoredPosition;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            tempData.OnUpdate = onUpdate;

            objToAnchoredPosition.IsLoop = false;
            objToAnchoredPosition.AddDataSequence(tempData);

            if (!objToAnchoredPosition.enabled)
            {
                objToAnchoredPosition.ElapsedDuration = 0;
                objToAnchoredPosition.TotalDuration = 0;
                objToAnchoredPosition.enabled = true;
            }

            return objToAnchoredPosition;
        }


        /// <summary>LayoutGroup 컴포넌트의 spacing 수치를 조절하는 함수</summary>
        public static TweenData LayoutGroupSpacing(GameObject targetObject, float targetValue, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null, Action onUpdate = null)
        {
            TweenLayoutGroupSpacing ObjToLayoutGroup = !targetObject.GetComponent<TweenLayoutGroupSpacing>()
                ? targetObject.AddComponent<TweenLayoutGroupSpacing>()
                : targetObject.GetComponent<TweenLayoutGroupSpacing>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetValue;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            tempData.OnUpdate = onUpdate;

            ObjToLayoutGroup.IsLoop = false;
            ObjToLayoutGroup.AddDataSequence(tempData);

            if (!ObjToLayoutGroup.enabled)
            {
                ObjToLayoutGroup.ElapsedDuration = 0;
                ObjToLayoutGroup.TotalDuration = 0;
                ObjToLayoutGroup.enabled = true;
            }

            return ObjToLayoutGroup;
        }



        /// <summary>목표 값으로 지속 시간동안 텍스트 컬러 값을 변경하는 함수</summary>
        public static TweenData TextColor(GameObject targetObject, Color targetColor, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null, Action onUpdate = null)
        {
            TweenTextColor objToColor = !targetObject.GetComponent<TweenTextColor>()
                ? targetObject.AddComponent<TweenTextColor>()
                : targetObject.GetComponent<TweenTextColor>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetColor;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            tempData.OnUpdate = onUpdate;

            objToColor.IsLoop = false;
            objToColor.AddDataSequence(tempData);

            if (!objToColor.enabled)
            {
                objToColor.ElapsedDuration = 0;
                objToColor.TotalDuration = 0;
                objToColor.enabled = true;
            }

            return objToColor;
        }


        /// <summary>목표 값으로 지속 시간동안 텍스트 알파 값을 변경하는 함수</summary>
        public static TweenData TextAlpha(GameObject targetObject, float targetAlpha, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null, Action onUpdate = null)
        {
            TweenTextAlpha objToColor = !targetObject.GetComponent<TweenTextAlpha>()
                ? targetObject.AddComponent<TweenTextAlpha>()
                : targetObject.GetComponent<TweenTextAlpha>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetAlpha;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            tempData.OnUpdate = onUpdate;

            objToColor.IsLoop = false;
            objToColor.AddDataSequence(tempData);

            if (!objToColor.enabled)
            {
                objToColor.ElapsedDuration = 0;
                objToColor.TotalDuration = 0;
                objToColor.enabled = true;
            }

            return objToColor;
        }


        /// <summary>목표 값으로 지속 시간동안 TMP 컬러 값을 변경하는 함수</summary>
        public static TweenData TMPColor(GameObject targetObject, Color targetColor, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null, Action onUpdate = null)
        {
            TweenTMPColor objToColor = !targetObject.GetComponent<TweenTMPColor>()
                ? targetObject.AddComponent<TweenTMPColor>()
                : targetObject.GetComponent<TweenTMPColor>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetColor;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            tempData.OnUpdate = onUpdate;

            objToColor.IsLoop = false;
            objToColor.AddDataSequence(tempData);

            if (!objToColor.enabled)
            {
                objToColor.ElapsedDuration = 0;
                objToColor.TotalDuration = 0;
                objToColor.enabled = true;
            }

            return objToColor;
        }


        /// <summary>목표 값으로 지속 시간동안 TMP 알파 값을 변경하는 함수</summary>
        public static TweenData TMPAlpha(GameObject targetObject, float targetAlpha, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null, Action onUpdate = null)
        {
            TweenTMPAlpha objToColor = !targetObject.GetComponent<TweenTMPAlpha>()
                ? targetObject.AddComponent<TweenTMPAlpha>()
                : targetObject.GetComponent<TweenTMPAlpha>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetAlpha;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            tempData.OnUpdate = onUpdate;

            objToColor.IsLoop = false;
            objToColor.AddDataSequence(tempData);

            if (!objToColor.enabled)
            {
                objToColor.ElapsedDuration = 0;
                objToColor.TotalDuration = 0;
                objToColor.enabled = true;
            }

            return objToColor;
        }


        /// <summary>목표 값으로 지속 시간동안 이미지 컬러 값을 변경하는 함수</summary>
        public static TweenData IamgeColor(GameObject targetObject, Color targetColor, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null, Action onUpdate = null)
        {
            TweenImageColor objToColor = !targetObject.GetComponent<TweenImageColor>()
                ? targetObject.AddComponent<TweenImageColor>()
                : targetObject.GetComponent<TweenImageColor>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetColor;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            tempData.OnUpdate = onUpdate;

            objToColor.IsLoop = false;
            objToColor.AddDataSequence(tempData);

            if (!objToColor.enabled)
            {
                objToColor.ElapsedDuration = 0;
                objToColor.TotalDuration = 0;
                objToColor.enabled = true;
            }

            return objToColor;
        }


        /// <summary>목표 값으로 지속 시간동안 이미지 알파 값을 변경하는 함수</summary>
        public static TweenData IamgeAlpha(GameObject targetObject, float targetAlpha, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null, Action onUpdate = null)
        {
            TweenImageAlpha objToColor = !targetObject.GetComponent<TweenImageAlpha>()
                ? targetObject.AddComponent<TweenImageAlpha>()
                : targetObject.GetComponent<TweenImageAlpha>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetAlpha;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            tempData.OnUpdate = onUpdate;

            objToColor.IsLoop = false;
            objToColor.AddDataSequence(tempData);

            if (!objToColor.enabled)
            {
                objToColor.ElapsedDuration = 0;
                objToColor.TotalDuration = 0;
                objToColor.enabled = true;
            }

            return objToColor;
        }


        /// <summary>목표 값으로 지속 시간동안 스프라이트 렌더러 컬러 값을 변경하는 함수</summary>
        public static TweenData SpriteRendererColor(GameObject targetObject, Color targetColor, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null, Action onUpdate = null)
        {
            TweenSpriteRendererColor objToColor = !targetObject.GetComponent<TweenSpriteRendererColor>()
                ? targetObject.AddComponent<TweenSpriteRendererColor>()
                : targetObject.GetComponent<TweenSpriteRendererColor>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetColor;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            tempData.OnUpdate = onUpdate;

            objToColor.IsLoop = false;
            objToColor.AddDataSequence(tempData);

            if (!objToColor.enabled)
            {
                objToColor.ElapsedDuration = 0;
                objToColor.TotalDuration = 0;
                objToColor.enabled = true;
            }

            return objToColor;
        }


        /// <summary>목표 값으로 지속 시간동안 스프라이트 렌더러 알파 값을 변경하는 함수</summary>
        public static TweenData SpriteRendererAlpha(GameObject targetObject, float targetAlpha, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null, Action onUpdate = null)
        {
            TweenSpriteRendererAlpha objToColor = !targetObject.GetComponent<TweenSpriteRendererAlpha>()
                ? targetObject.AddComponent<TweenSpriteRendererAlpha>()
                : targetObject.GetComponent<TweenSpriteRendererAlpha>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetAlpha;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            tempData.OnUpdate = onUpdate;

            objToColor.IsLoop = false;
            objToColor.AddDataSequence(tempData);

            if (!objToColor.enabled)
            {
                objToColor.ElapsedDuration = 0;
                objToColor.TotalDuration = 0;
                objToColor.enabled = true;
            }

            return objToColor;
        }


        /// <summary>목표 값으로 지속 시간동안 캔버스 그룹 알파 값을 변경하는 함수</summary>
        public static TweenData CanvasGroupAlpha(GameObject targetObject, float targetAlpha, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null, Action onUpdate = null)
        {
            TweenCanvasGroupAlpha objToAlpha = !targetObject.GetComponent<TweenCanvasGroupAlpha>()
                ? targetObject.AddComponent<TweenCanvasGroupAlpha>()
                : targetObject.GetComponent<TweenCanvasGroupAlpha>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetAlpha;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            tempData.OnUpdate = onUpdate;

            objToAlpha.IsLoop = false;
            objToAlpha.AddDataSequence(tempData);

            if (!objToAlpha.enabled)
            {
                objToAlpha.ElapsedDuration = 0;
                objToAlpha.TotalDuration = 0;
                objToAlpha.enabled = true;
            }

            return objToAlpha;
        }


        /// <summary>목표 값으로 지속 시간동안 카메라 사이즈 값을 변경하는 함수</summary>
        public static TweenData CameraOrthographicSize(GameObject targetObject, float targetSize, float duration, TweenMode tweenMode = TweenMode.Constant, Action onComplete = null, Action onUpdate = null)
        {
            TweenCameraSize objToColor = !targetObject.GetComponent<TweenCameraSize>()
                ? targetObject.AddComponent<TweenCameraSize>()
                : targetObject.GetComponent<TweenCameraSize>();

            DataSequence tempData = new DataSequence();
            tempData.TargetValue = targetSize;
            tempData.Duration = duration;
            tempData.TweenMode = tweenMode;
            tempData.OnComplete = onComplete;
            tempData.OnUpdate = onUpdate;

            objToColor.IsLoop = false;
            objToColor.AddDataSequence(tempData);

            if (!objToColor.enabled)
            {
                objToColor.ElapsedDuration = 0;
                objToColor.TotalDuration = 0;
                objToColor.enabled = true;
            }

            return objToColor;
        }

    }
}

