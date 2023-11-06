using UnityEngine;

namespace Muks.Tween
{
    public class TweenMove : TweenData
    {

        /// <summary> ��ǥ ��ġ </summary>
        public Vector3 TargetPosition;

        /// <summary> ���� ��ġ</summary>
        public Vector3 StartPosition;

        public override void SetData(DataSequence dataSequence)
        {
            StartPosition = (Vector3)dataSequence.StartObject;
            TargetPosition = (Vector3)dataSequence.TargetObject;
            TotalDuration = dataSequence.Duration;
            TweenMode = dataSequence.TweenMode;
            OnComplete = dataSequence.OnComplete;
        }

        protected override void Update()
        {
            base.Update();

            float percent = _percentHandler[TweenMode](ElapsedDuration, TotalDuration);

            transform.position = Vector3.Lerp(StartPosition, TargetPosition, percent);

        }
    }
}
