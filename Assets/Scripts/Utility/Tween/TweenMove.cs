using UnityEngine;

namespace Muks.Tween
{
    public class TweenMove : TweenData
    {

        /// <summary> 목표 위치 </summary>
        public Vector3 TargetPosition;

        /// <summary> 시작 위치</summary>
        public Vector3 StartPosition;


        protected override void Update()
        {
            base.Update();

            float percent = _percentHandler[TweenMode](ElapsedDuration, TotalDuration);

            transform.position = Vector3.Lerp(StartPosition, TargetPosition, percent);

        }
    }
}
