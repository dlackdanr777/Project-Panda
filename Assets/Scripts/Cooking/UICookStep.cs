using UnityEngine;

namespace Cooking
{
    public abstract class UICookStep : MonoBehaviour
    {
        protected UICook _uiCook;

        protected CookSystem _cookSystem => _uiCook.CookSystem;


        public virtual void Init(UICook uiCook)
        {
            _uiCook = uiCook;
        }

        /// <summary>���� �ܰ踦 �����ϴ� �Լ�</summary>
        public abstract void StartStep();

        /// <summary>���� �ܰ踦 �����ϴ� �Լ�</summary>
        public abstract void StopStep();

        /// <summary>���� �ܰ����� �Ѿ �� �ֳ� ���� Ȯ���ϴ� �Լ�</summary>
        public abstract bool CheckNextStep();
    }
}


