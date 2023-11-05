using System;
using System.Collections.Generic;
using UnityEngine;

namespace Muks.Tween
{
    public class TweenData : MonoBehaviour
    {
        ///  <summary> 현재 경과 시간 </summary>
        public float ElapsedDuration;

        ///  <summary> 총 경과 시간 </summary>
        public float TotalDuration;

        ///  <summary> 콜백 함수 </summary>
        public Action OnComplete;

        public TweenMode TweenMode;

        protected Dictionary<TweenMode, Func<float, float, float>> _percentHandler;


        private void Awake()
        {
            _percentHandler = new Dictionary<TweenMode, Func<float, float, float>>
        {
            { TweenMode.Constant, ConstantSpeed },
                {TweenMode.Quadratic, Quadratic },
            { TweenMode.Smoothstep, Smoothstep },
            { TweenMode.Smootherstep, Smootherstep },
                {TweenMode.Spike, Spike },
            { TweenMode.Sinerp, Sinerp },
            { TweenMode.Coserp, Coserp }
        };

        }


        protected virtual void Update()
        {
            ElapsedDuration += Time.deltaTime;

            //현재 경과 시간이 총 경과시간을 넘었을때
            if (ElapsedDuration > TotalDuration)
            {
                enabled = false;
                OnComplete?.Invoke();
            }
        }

        //등속운동
        private float ConstantSpeed(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;
            return percent;
        }

        private float Quadratic(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;
            percent = percent * percent;
            return percent;
        }


        //스무스하게 움직임
        private float Smoothstep(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;
            percent = percent * percent * (3f - 2f * percent);
            return percent;
        }

        //더욱더 스무스하게 움직임
        private float Smootherstep(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;
            percent = percent * percent * percent * (percent * (6f * percent - 15f) + 10f);
            return percent;
        }

        private float Spike(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;
            if(elapsedDuration <= totalDuration * 0.5f)
                return  Mathf.Pow(percent/ 0.5f, 3);

            return Mathf.Pow((1 - percent) / 0.5f, 3);
        }


        //sin그래프처럼 움직임
        private float Sinerp(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;
            percent = Mathf.Sin(percent * Mathf.PI * 0.5f);
            return percent;
        }

        //cos그래프처럼 움직임
        private float Coserp(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;
            percent = Mathf.Cos(percent * Mathf.PI * 0.5f);
            return percent;
        }
    }
}
