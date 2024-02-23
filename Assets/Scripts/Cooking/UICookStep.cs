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

        /// <summary>현재 단계를 시작하는 함수</summary>
        public abstract void StartStep();

        /// <summary>현재 단계를 중지하는 함수</summary>
        public abstract void StopStep();
    }
}


