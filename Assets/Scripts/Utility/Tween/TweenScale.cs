
using UnityEngine;

namespace Muks.Tween
{
    public class TweenScale : TweenData
    {
        /// <summary> 목표 회전 값 </summary>
        public Vector3 TargetScale;

        /// <summary> 시작 회전 값</summary>
        public Vector3 StartScale;
        public override void SetData(DataSequence dataSequence)
        {
            StartScale = (Vector3)dataSequence.StartObject;
            TargetScale = (Vector3)dataSequence.TargetObject;
            TotalDuration = dataSequence.Duration;
            TweenMode = dataSequence.TweenMode;
            OnComplete = dataSequence.OnComplete;
        }

        protected override void Update()
        {
            base.Update();

            float percent = _percentHandler[TweenMode](ElapsedDuration, TotalDuration);

            transform.localScale = Vector3.Lerp(StartScale, TargetScale, percent);
        }
    }
}
