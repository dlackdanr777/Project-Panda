using UnityEngine;
using UnityEngine.UI;

namespace Muks.Tween
{
    public class TweenColor : TweenData
    {
        /// <summary> ��ǥ ��ġ </summary>
        public Color TargetColor;

        /// <summary> ���� ��ġ</summary>
        public Color StartColor;

        public Image Image;

        protected override void Update()
        {
            base.Update();

            float percent = _percentHandler[TweenMode](ElapsedDuration, TotalDuration);
            
            Image.color = Color.Lerp(StartColor, TargetColor, percent);
        }
    }
}

