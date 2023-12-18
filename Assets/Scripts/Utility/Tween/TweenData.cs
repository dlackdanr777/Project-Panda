using System;
using System.Collections.Generic;
using UnityEngine;

namespace Muks.Tween
{
    public struct DataSequence
    {
        public object StartValue;
        public object TargetValue;
        public float Duration;
        public TweenMode TweenMode;
        public Action OnComplete;
    }

    public class TweenData : MonoBehaviour
    {
        public Queue<DataSequence> DataSequences;

        ///  <summary> 현재 경과 시간 </summary>
        public float ElapsedDuration;

        ///  <summary> 총 경과 시간 </summary>
        public float TotalDuration;

        ///  <summary> 콜백 함수 </summary>
        public Action OnComplete;

        public TweenMode TweenMode;



        protected Dictionary<TweenMode, Func<float, float, float>> _percentHandler;

        public virtual void SetData(DataSequence dataSequence)
        {
            TotalDuration = dataSequence.Duration;
            TweenMode = dataSequence.TweenMode;
            OnComplete = dataSequence.OnComplete;
        }


        private void Awake()
        {
            DataSequences = new Queue<DataSequence>();
            _percentHandler = new Dictionary<TweenMode, Func<float, float, float>>
            {
                { TweenMode.Constant, Constant },
                {TweenMode.Quadratic, Quadratic },
                { TweenMode.Smoothstep, Smoothstep },
                { TweenMode.Smootherstep, Smootherstep },
                {TweenMode.Spike, Spike },
                {TweenMode.EaseInOutElastic, EaseInOutElastic },
                {TweenMode.EaseInOutBack, EaseInOutBack },
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
                OnComplete?.Invoke();
                OnComplete = null;

                if (DataSequences.Count > 0)
                {
                    ElapsedDuration = 0;
                    SetData(DataSequences.Dequeue());
                }
                else
                {
                    enabled = false;
                }
            }
        }

        public void AddDataSequence(DataSequence dataSequence)
        {
            DataSequences.Enqueue(dataSequence);
        }


        //등속운동
        private float Constant(float elapsedDuration, float totalDuration)
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


        //더욱 더 스무스하게 움직임
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


        private float EaseInOutElastic(float elapsedDuration, float totalDuration)
        {
            float c = (2 * Mathf.PI) / 4.5f;
            float percent = elapsedDuration / totalDuration;

            return percent == 0
            ? 0
            : percent == 1
            ? 1
            : percent < 0.5f
            ? -(Mathf.Pow(2, 20 * percent - 10) * Mathf.Sin((20 * percent - 11.125f) * c)) / 2
            : (Mathf.Pow(2, -20 * percent + 10) * Mathf.Sin((20 * percent - 11.125f) * c)) / 2 + 1;

        }


        private float EaseInOutBack(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;
            float c1 = 1.70158f;
            float c2 = c1 * 1.525f;
            return percent < 0.5f
               ? (Mathf.Pow(2 * percent, 2) * ((c2 + 1) * 2 * percent - c2)) / 2
               : (Mathf.Pow(2 * percent - 2, 2) * ((c2 + 1) * (percent * 2 - 2) + c2) + 2) / 2;
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
