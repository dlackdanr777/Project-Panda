using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Muks.Tween
{
    public struct DataSequence
    {
        public object StartValue;
        public object TargetValue;
        public float Duration;
        public TweenMode TweenMode;
        public Action OnUpdate;
        public Action OnComplete;
    }


    public enum LoopType
    {
        /// <summary>처음으로 되돌아가서 다시 시작</summary>
        Restart,
        /// <summary>실행된 모션 되감고 다시 시작</summary>
        Yoyo
    }

    public class TweenData : MonoBehaviour
    {
        public Queue<DataSequence> DataSequences = new Queue<DataSequence>();

        ///  <summary> 현재 경과 시간 </summary>
        public float ElapsedDuration;

        ///  <summary> 총 경과 시간 </summary>
        public float TotalDuration;

        ///  <summary> 실행 중 주기적으로 실행되는 콜백 함수 </summary>
        public Action OnUpdate;

        ///  <summary> 종료 콜백 함수 </summary>
        public Action OnComplete;

        public TweenMode TweenMode;

        public bool IsLoop;

        protected Dictionary<TweenMode, Func<float, float, float>> _percentHandler;

        private LoopType _loopType;

        private bool _isRightMove = true;


        public virtual void SetData(DataSequence dataSequence)
        {
            TotalDuration = dataSequence.Duration;
            TweenMode = dataSequence.TweenMode;
            OnUpdate = dataSequence.OnUpdate;
            OnComplete = dataSequence.OnComplete;
        }


        /// <summary>무한 반복</summary>
        public void Loop(LoopType loopType = LoopType.Restart)
        {
            DataSequence sequence = DataSequences.Last();
            DataSequences.Clear();
            DataSequences.Enqueue(sequence);
            SetData(DataSequences.Dequeue());
            _loopType = loopType;
            IsLoop = true;
        }


        /// <summary>반복 횟수 설정</summary>
        public void Repeat(int count)
        {
            DataSequence sequence = DataSequences.Last();
            for (int i = 1; i < count; i++)
            {
                AddDataSequence(sequence);
            }
        }


        private void Awake()
        {
            _percentHandler = new Dictionary<TweenMode, Func<float, float, float>>
            {
                { TweenMode.Constant, Constant },
                {TweenMode.Quadratic, Quadratic },
                { TweenMode.Smoothstep, Smoothstep },
                { TweenMode.Smootherstep, Smootherstep },
                {TweenMode.Spike, Spike },
                {TweenMode.EaseInQuint, EaseInQuint },
                {TweenMode.EaseOutQuint, EaseOutQuint },
                {TweenMode.EaseInOutQuint, EaseInOutQuint },
                {TweenMode.EaseInExpo, EaseInExpo },
                {TweenMode.EaseOutExpo, EaseOutExpo },
                {TweenMode.EaseInOutExpo, EaseInOutExpo },
                {TweenMode.EaseInElastic, EaseInElastic },
                {TweenMode.EaseOutElastic, EaseOutElastic },
                {TweenMode.EaseInOutElastic, EaseInOutElastic },
                {TweenMode.EaseInBack, EaseInBack },
                {TweenMode.EaseOutBack, EaseOutBack },
                {TweenMode.EaseInOutBack, EaseInOutBack },
                {TweenMode.EaseInBounce, EaseInBounce },
                {TweenMode.EaseOutBounce, EaseOutBounce },
                {TweenMode.EaseInOutBounce, EaseInOutBounce },
                { TweenMode.Sinerp, Sinerp },
                { TweenMode.Coserp, Coserp }
            };
        }


        protected virtual void Update()
        {
            //만약 반복 설정이 되어있다면?
            if (IsLoop)
            {
                switch (_loopType)
                {
                    case LoopType.Restart:
                        ElapsedDuration += Time.deltaTime;
                        ElapsedDuration = Mathf.Clamp(ElapsedDuration, 0, TotalDuration);

                        if (TotalDuration <= ElapsedDuration)
                            ElapsedDuration = 0;
                        break;

                    case LoopType.Yoyo:
                        ElapsedDuration += _isRightMove ? Time.deltaTime : -Time.deltaTime;
                        ElapsedDuration = Mathf.Clamp(ElapsedDuration, 0, TotalDuration);

                        if (_isRightMove && TotalDuration <= ElapsedDuration)
                        {
                            _isRightMove = false;
                        }
                        else if (!_isRightMove && ElapsedDuration <= 0)
                        {
                            _isRightMove = true;
                        }
                        break;
                }   
            }

            else
            {
                ElapsedDuration += Time.deltaTime;
                ElapsedDuration = Mathf.Clamp(ElapsedDuration, 0, TotalDuration);

                //현재 경과 시간이 총 경과시간을 넘었을때
                if (TotalDuration <= ElapsedDuration)
                {

                    OnComplete?.Invoke();
                    OnComplete = null;
                    OnUpdate = null;

                    if (0 < DataSequences.Count)
                    {
                        ElapsedDuration = 0;
                        SetData(DataSequences.Dequeue());
                    }
                    else
                    {
                        TweenCompleted();
                        enabled = false;
                    }
                }
            }

            OnUpdate?.Invoke();
          
        }


        /// <summary>Tween애니메이션이 종료될 경우 불러오는 함수 </summary>
        protected virtual void TweenCompleted()
        {
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


        private float Smoothstep(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;
            percent = percent * percent * (3f - 2f * percent);

            return percent;
        }


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


        private float EaseInQuint(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;

            return percent * percent * percent * percent * percent;
        }


        private float EaseOutQuint(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;

            return 1 - Mathf.Pow((1 - percent), 5);
        }


        private float EaseInOutQuint(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;

            return percent < 0.5f ? 16 * percent * percent * percent * percent * percent
                : 1 - Mathf.Pow(-2 * percent + 2, 5) / 2;
        }


        private float EaseInExpo(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;

            return percent == 0 ? 0 : Mathf.Pow(2, 10 * percent - 10);
        }


        private float EaseOutExpo(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;

            return percent == 1 ? 1 : 1 - Mathf.Pow(2, -10 * percent);
        }


        private float EaseInOutExpo(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;

            float returnValue = percent == 0
                ? 0
                : percent == 1
                ? 1
                : percent < 0.5f ? Mathf.Pow(2, 20 * percent - 10) / 2
                : (2 - Mathf.Pow(2, -20 * percent + 10)) / 2;

            return returnValue;
        }


        private float EaseInElastic(float elapsedDuration, float totalDuration)
        {
            float c = (2 * Mathf.PI) / 3f;
            float percent = elapsedDuration / totalDuration;

            return percent == 0 ? 0 : percent == 1 ?
                1: -Mathf.Pow(2, 10 * percent - 10) * Mathf.Sin((percent * 10 - 10.75f) * c);
        }


        private float EaseOutElastic(float elapsedDuration, float totalDuration)
        {
            float c = (2 * Mathf.PI) / 3f;
            float percent = elapsedDuration / totalDuration;

            return percent == 0 ? 0 : percent == 1 ?
                1 : Mathf.Pow(2, -10 * percent) * Mathf.Sin((percent * 10 - 0.75f) * c) + 1;
        }


        private float EaseInOutElastic(float elapsedDuration, float totalDuration)
        {
            float c = (2 * Mathf.PI) / 4.5f;
            float percent = elapsedDuration / totalDuration;

            return percent == 0 ? 0 : percent == 1 ? 1 : percent < 0.5f
            ? -(Mathf.Pow(2, 20 * percent - 10) * Mathf.Sin((20 * percent - 11.125f) * c)) / 2
            : (Mathf.Pow(2, -20 * percent + 10) * Mathf.Sin((20 * percent - 11.125f) * c)) / 2 + 1;
        }


        private float EaseInBack(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;
            float c1 = 1.70158f;
            float c2 = c1 + 1f;

            return c2 * percent * percent * percent - c1 * percent * percent;
        }


        private float EaseOutBack(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;
            float c1 = 1.70158f;
            float c2 = c1 + 1f;

            return 1 + c2 * Mathf.Pow(percent - 1, 3) + c1 * Mathf.Pow(percent - 1, 2);
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


        private float EaseInBounce(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;
            return 1 - EaseOutBounce(1 - percent);
        }


        private float EaseOutBounce(float percent)
        {
            float n1 = 7.5625f;
            float d1 = 2.75f;
            if (percent < 1 / d1)
            {
                return n1 * percent * percent;
            }
            else if (percent < 2 / d1)
            {
                return n1 * (percent -= 1.5f / d1) * percent + 0.75f;
            }
            else if (percent < 2.5f / d1)
            {
                return n1 * (percent -= 2.25f / d1) * percent + 0.9375f;
            }
            else
            {
                return n1 * (percent -= 2.625f / d1) * percent + 0.984375f;
            }
        }


        private float EaseOutBounce(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;

            float n1 = 7.5625f;
            float d1 = 2.75f;
            if (percent < 1 / d1)
            {
                return n1 * percent * percent;
            }
            else if (percent < 2 / d1)
            {
                return n1 * (percent -= 1.5f / d1) * percent + 0.75f;
            }
            else if (percent < 2.5f / d1)
            {
                return n1 * (percent -= 2.25f / d1) * percent + 0.9375f;
            }
            else
            {
                return n1 * (percent -= 2.625f / d1) * percent + 0.984375f;
            }
        }


        private float EaseInOutBounce(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;

            return percent < 0.5f
               ? (1 - EaseOutBounce(1 - 2 * percent)) / 2
               : (1 + EaseOutBounce(2 * percent - 1)) / 2;
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
