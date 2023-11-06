using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Muks.Tween
{
    public class TweenColor : TweenData
    {
        /// <summary> 목표 위치 </summary>
        public Color TargetColor;

        /// <summary> 시작 위치</summary>
        public Color StartColor;

        public Image Image;

        public override void SetData(DataSequence dataSequence)
        {
            Image = (Image)dataSequence.Object;
            StartColor = (Color)dataSequence.StartObject;
            TargetColor = (Color)dataSequence.TargetObject;
            TotalDuration = dataSequence.Duration;
            TweenMode = dataSequence.TweenMode;
            OnComplete = dataSequence.OnComplete;
        }

        protected override void Update()
        {
            base.Update();

            float percent = _percentHandler[TweenMode](ElapsedDuration, TotalDuration);
            
            Image.color = Color.Lerp(StartColor, TargetColor, percent);
        }
    }
}

