using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Muks.Tween
{
    public class TweenCanvasGroupAlpha : TweenData
    {
        /// <summary> 시작 위치</summary>
        public float StartAlpha;

        public float TargetAlpha;

        public CanvasGroup _canvasGroup;


        public override void SetData(DataSequence dataSequence)
        {
            base.SetData(dataSequence);
            if(TryGetComponent(out _canvasGroup))
            {
                TargetAlpha = (float)dataSequence.TargetValue;
                StartAlpha = _canvasGroup.alpha;
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
            
            _canvasGroup.alpha = Mathf.LerpUnclamped(StartAlpha, TargetAlpha, percent);
        }


        protected override void TweenCompleted()
        {
            if (TweenMode != TweenMode.Spike)
                _canvasGroup.alpha = TargetAlpha;
        }
    }
}

