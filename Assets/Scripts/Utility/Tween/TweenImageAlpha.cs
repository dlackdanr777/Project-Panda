using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Muks.Tween
{
    public class TweenImageAlpha : TweenData
    {
        /// <summary> 목표 위치 </summary>
        public Color TargetColor;

        /// <summary> 시작 위치</summary>
        public Color StartColor;

        public float TargetAlpha;

        public Image Image;

        public override void SetData(DataSequence dataSequence)
        {
            base.SetData(dataSequence);
            if(TryGetComponent(out Image))
            {
                TargetAlpha = (float)dataSequence.TargetValue;
                StartColor = Image.color;
                TargetColor = Image.color;
                TargetColor.a = TargetAlpha;
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

            Image.color = Color.LerpUnclamped(StartColor, TargetColor, percent);
        }

        protected override void TweenCompleted()
        {
            if(TweenMode != TweenMode.Spike)
                Image.color = TargetColor;
        }
    }
}

