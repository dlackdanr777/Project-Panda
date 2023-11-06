
using UnityEngine;

namespace Muks.Tween
{
    public class TweenRotate : TweenData
    {
        /// <summary> 목표 회전 값 </summary>
        public Vector3 TargetEulerAngles;

        /// <summary> 시작 회전 값</summary>
        public Vector3 StartEulerAngles;


        protected override void Update()
        {
            base.Update();

            float percent = _percentHandler[TweenMode](ElapsedDuration, TotalDuration);

            transform.eulerAngles = Vector3.Lerp(StartEulerAngles, TargetEulerAngles, percent);

        }
    }
}
