
using UnityEngine;

namespace Muks.Tween
{
    public class TweenScale : TweenData
    {
        /// <summary> 목표 회전 값 </summary>
        public Vector3 TargetScale;

        /// <summary> 시작 회전 값</summary>
        public Vector3 StartScale;


        protected override void Update()
        {
            base.Update();

            float percent = _percentHandler[TweenMode](ElapsedDuration, TotalDuration);

            transform.localScale = Vector3.Lerp(StartScale, TargetScale, percent);
        }
    }
}
