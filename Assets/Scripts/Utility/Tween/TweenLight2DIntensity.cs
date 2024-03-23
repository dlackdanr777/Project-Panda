using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Muks.Tween
{
    public class TweenLight2DIntensity : TweenData
    {
        /// <summary> ��ǥ ��ġ </summary>
        public float TargetIntensity;

        /// <summary> ���� ��ġ</summary>
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
                Debug.LogError("�ʿ� ������Ʈ�� �������� �ʽ��ϴ�.");
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

