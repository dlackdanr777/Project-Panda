using UnityEngine;
using UnityEngine.UI;

namespace Muks.Tween
{
    public class TweenLayoutGroupSpacing : TweenData
    {
        public float StartValue;
        public float TargetValue;

        private HorizontalOrVerticalLayoutGroup _layoutGroup;

        public override void SetData(DataSequence dataSequence)
        {
            base.SetData(dataSequence);

            if(TryGetComponent(out _layoutGroup))
            {
                StartValue = _layoutGroup.spacing;
                TargetValue = (float)dataSequence.TargetValue;
            }
            else
            {
                Debug.LogError("컴포넌트가 존재하지 않습니다.");
            }
        }


        protected override void Update()
        {
            base.Update();

            float percent = _percentHandler[TweenMode](ElapsedDuration, TotalDuration);

            _layoutGroup.spacing = Mathf.LerpUnclamped(StartValue, TargetValue, percent);
        }


        protected override void TweenCompleted()
        {
            if (TweenMode != TweenMode.Spike)
                _layoutGroup.spacing = TargetValue;
        }
    }
}
