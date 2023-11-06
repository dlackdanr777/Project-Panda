using UnityEngine;

namespace Muks.Tween
{
    public class TweenMove : TweenData
    {
        public Vector3 StartPosition;
        public Vector3 TargetPosition;

        public override void SetData(DataSequence dataSequence)
        {
            base.SetData(dataSequence);

            StartPosition = transform.position;
            TargetPosition = (Vector3)dataSequence.TargetValue;
        }

        protected override void Update()
        {
            base.Update();

            float percent = _percentHandler[TweenMode](ElapsedDuration, TotalDuration);

            transform.position = Vector3.LerpUnclamped(StartPosition, TargetPosition, percent);

        }
    }
}
