using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Muks.Tween
{
    public class TweenLight2DIntensity : TweenData
    {
        /// <summary> 목표 위치 </summary>
        public float TargetIntensity;

        /// <summary> 시작 위치</summary>
        public float StartIntensity;


        public Light2D Light2D;

        public override void SetData(DataSequence dataSequence)
        {
            base.SetData(dataSequence);
            if(TryGetComponent(out Light2D))
            {
                TargetIntensity = (float)dataSequence.TargetValue;
                StartIntensity = Light2D.intensity;
            }
            else
            {
                Debug.LogError("필요 컴포넌트가 존재하지 않습니다.");
            }
        }

        protected override void Update()
        {
            base.Update();

            float percent = _percentHandler[TweenMode](ElapsedDuration, TotalDuration);

            Light2D.intensity = Mathf.LerpUnclamped(StartIntensity, TargetIntensity, percent);
        }

        protected override void TweenCompleted()
        {
            if(TweenMode != TweenMode.Spike)
                Light2D.intensity = TargetIntensity;
        }
    }
}

