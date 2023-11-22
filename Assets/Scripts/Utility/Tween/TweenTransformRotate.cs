
using TMPro;
using UnityEngine;

namespace Muks.Tween
{
    public class TweenTransformRotate : TweenData
    {
        /// <summary> 목표 회전 값 </summary>
        public Vector3 TargetEulerAngles;

        /// <summary> 시작 회전 값</summary>
        public Vector3 StartEulerAngles;

        public override void SetData(DataSequence dataSequence)
        {
            base.SetData(dataSequence);

            StartEulerAngles = transform.eulerAngles;
            TargetEulerAngles = (Vector3)dataSequence.TargetValue;
        }

        protected override void Update()
        {
            base.Update();

            float percent = _percentHandler[TweenMode](ElapsedDuration, TotalDuration);

            transform.eulerAngles = Vector3.LerpUnclamped(StartEulerAngles, TargetEulerAngles, percent);

        }
    }
}
