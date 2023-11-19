using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Muks.Tween
{
    public class TweenTextColor : TweenData
    {
        /// <summary> ��ǥ ��ġ </summary>
        public Color TargetColor;

        /// <summary> ���� ��ġ</summary>
        public Color StartColor;

        public Text Text;

        public override void SetData(DataSequence dataSequence)
        {
            base.SetData(dataSequence);
            if(TryGetComponent(out Text))
            {
                StartColor = Text.color;
                TargetColor = (Color)dataSequence.TargetValue;
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
            
            Text.color = Color.LerpUnclamped(StartColor, TargetColor, percent);
        }
    }
}
