
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
