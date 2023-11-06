using System;
using System.Collections.Generic;
using UnityEngine;
using static Muks.Tween.TweenData;

namespace Muks.Tween
{
    public struct DataSequence
    {
        public object Object;
        public object StartObject;
        public object TargetObject;
        public float Duration;
        public TweenMode TweenMode;
        public Action OnComplete;
    }

    public abstract class TweenData : MonoBehaviour
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


        private void Awake()
        {
            DataSequences = new Queue<DataSequence>();
            _percentHandler = new Dictionary<TweenMode, Func<float, float, float>>
        {
            { TweenMode.Constant, ConstantSpeed },
                {TweenMode.Quadratic, Quadratic },
            { TweenMode.Smoothstep, Smoothstep },
            { TweenMode.Smootherstep, Smootherstep },
                {TweenMode.Spike, Spike },
                {TweenMode.Back, Back },
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
                if(DataSequences.Count > 0)
                {
                    ElapsedDuration = 0;
                    SetData(DataSequences.Dequeue());
                }
                else
                {
                    enabled = false;
                }
                OnComplete?.Invoke();
            }
        }

        public void SetDataSequence(DataSequence dataSequence)
        {
            DataSequences.Enqueue(dataSequence);
        }


        public abstract void SetData(DataSequence dataSequence);

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

        private float Back(float elapsedDuration, float totalDuration)
        {
            float c1 = 1.70158F;
            float c2 = c1 * 1.525F;
            float percent = elapsedDuration / totalDuration;
            return percent < 0.5f
              ? (Mathf.Pow(2 * percent, 2) * ((c2 + 1) * 2 * percent - c2)) / 2
              : (Mathf.Pow(2 * percent - 2, 2) * ((c2 + 1) * (percent * 2 - 2) + c2) + 2) / 2;

        }

        private float Bounce(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;
            percent = 1 - Mathf.Abs(Mathf.Cos(4 * Mathf.PI * percent));
            return percent;
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
