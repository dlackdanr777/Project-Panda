using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Muks.Tween
{
    public class TweenSpriteRendererColor : TweenData
    {
        /// <summary> 목표 위치 </summary>
        public Color TargetColor;

        /// <summary> 시작 위치</summary>
        public Color StartColor;

        public SpriteRenderer SpriteRenderer;


        public override void SetData(DataSequence dataSequence)
        {
            base.SetData(dataSequence);
            if(TryGetComponent(out SpriteRenderer))
            {
                StartColor = SpriteRenderer.color;
                TargetColor = (Color)dataSequence.TargetValue;
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

            SpriteRenderer.color = Color.LerpUnclamped(StartColor, TargetColor, percent);
        }


        protected override void TweenCompleted()
        {
            if (TweenMode != TweenMode.Spike)
                SpriteRenderer.color = TargetColor;
        }
    }
}

