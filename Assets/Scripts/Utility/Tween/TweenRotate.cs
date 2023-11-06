
using UnityEngine;

namespace Muks.Tween
{
    public class TweenRotate : TweenData
    {
        /// <summary> ��ǥ ȸ�� �� </summary>
        public Vector3 TargetEulerAngles;

        /// <summary> ���� ȸ�� ��</summary>
        public Vector3 StartEulerAngles;

        public override void SetData(DataSequence dataSequence)
        {
            StartEulerAngles = (Vector3)dataSequence.StartObject;
            TargetEulerAngles = (Vector3)dataSequence.TargetObject;
            TotalDuration = dataSequence.Duration;
            TweenMode = dataSequence.TweenMode;
            OnComplete = dataSequence.OnComplete;
        }

        protected override void Update()
        {
            base.Update();

            float percent = _percentHandler[TweenMode](ElapsedDuration, TotalDuration);

            transform.eulerAngles = Vector3.Lerp(StartEulerAngles, TargetEulerAngles, percent);

        }
    }
}
