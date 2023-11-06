
using UnityEngine;


namespace Muks.Tween
{
    /// <summary>
    /// Recttransform�� width�� height�� �����ϴ� Tween
    /// </summary>
    public class TweenSizeDelta : TweenData
    {
        /// <summary> ��ǥ ȸ�� �� </summary>
        public Vector2 TargetSizeDelta;

        /// <summary> ���� ȸ�� ��</summary>
        public Vector2 StartSizeDelta;

        public RectTransform RectTransform;

        public override void SetData(DataSequence dataSequence)
        {
            RectTransform = (RectTransform)dataSequence.Object;
            StartSizeDelta = (Vector2)dataSequence.StartObject;
            TargetSizeDelta = (Vector2)dataSequence.TargetObject;
            TotalDuration = dataSequence.Duration;
            TweenMode = dataSequence.TweenMode;
            OnComplete = dataSequence.OnComplete;
        }

        protected override void Update()
        {
            base.Update();

            float percent = _percentHandler[TweenMode](ElapsedDuration, TotalDuration);

            float width = Mathf.Lerp(StartSizeDelta.x, TargetSizeDelta.x, percent);
            float height = Mathf.Lerp(StartSizeDelta.y, TargetSizeDelta.y, percent);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
    }
}
