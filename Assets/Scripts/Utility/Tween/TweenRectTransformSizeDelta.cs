
using UnityEngine;


namespace Muks.Tween
{
    /// <summary>
    /// Recttransform의 width와 height를 조절하는 Tween
    /// </summary>
    public class TweenRectTransformSizeDelta : TweenData
    {
        /// <summary> 목표 회전 값 </summary>
        public Vector2 TargetSizeDelta;

        /// <summary> 시작 회전 값</summary>
        public Vector2 StartSizeDelta;

        public RectTransform RectTransform;


        public override void SetData(DataSequence dataSequence)
        {
            base.SetData(dataSequence);

            if (TryGetComponent(out RectTransform))
            {
                StartSizeDelta = RectTransform.sizeDelta;
                TargetSizeDelta = (Vector2)dataSequence.TargetValue;
            }
            else
            {
                Debug.LogError("필요 컴포넌트가 존재하지 않습니다.");
            }
        }


        protected override void Update()
        {
            base.Update();

            float percent = _percentHandler[TweenMode](ElapsedDuration, TotalDuration);

            float width = Mathf.LerpUnclamped(StartSizeDelta.x, TargetSizeDelta.x, percent);
            float height = Mathf.LerpUnclamped(StartSizeDelta.y, TargetSizeDelta.y, percent);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }


        protected override void TweenCompleted()
        {
            if (TweenMode != TweenMode.Spike)
            {
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, TargetSizeDelta.x);
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TargetSizeDelta.y);
            }  
        }
    }
}
