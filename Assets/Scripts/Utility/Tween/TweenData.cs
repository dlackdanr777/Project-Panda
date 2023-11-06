using System;
using System.Collections.Generic;
using UnityEngine;

namespace Muks.Tween
{
    public class TweenData : MonoBehaviour
    {
        ///  <summary> ���� ��� �ð� </summary>
        public float ElapsedDuration;

        ///  <summary> �� ��� �ð� </summary>
        public float TotalDuration;

        ///  <summary> �ݹ� �Լ� </summary>
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

            //���� ��� �ð��� �� ����ð��� �Ѿ�����
            if (ElapsedDuration > TotalDuration)
            {
                enabled = false;
                OnComplete?.Invoke();
            }
        }

        //��ӿ
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


        //�������ϰ� ������
        private float Smoothstep(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;
            percent = percent * percent * (3f - 2f * percent);
            return percent;
        }

        //����� �������ϰ� ������
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


        //sin�׷���ó�� ������
        private float Sinerp(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;
            percent = Mathf.Sin(percent * Mathf.PI * 0.5f);
            return percent;
        }

        //cos�׷���ó�� ������
        private float Coserp(float elapsedDuration, float totalDuration)
        {
            float percent = elapsedDuration / totalDuration;
            percent = Mathf.Cos(percent * Mathf.PI * 0.5f);
            return percent;
        }
    }
}
