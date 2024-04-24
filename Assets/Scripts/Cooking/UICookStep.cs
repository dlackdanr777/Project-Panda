using UnityEngine;

namespace Cooking
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UICookStep : MonoBehaviour
    {
        protected UICook _uiCook;

        protected CookSystem _cookSystem => _uiCook.CookSystem;

        protected CanvasGroup _canvasGroup;


        public virtual void Init(UICook uiCook)
        {
            _uiCook = uiCook;
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>���� �ܰ踦 �����ϴ� �Լ�</summary>
        public abstract void StartStep();

        /// <summary>���� �ܰ踦 �����ϴ� �Լ�</summary>
        public abstract void StopStep();
    }
}


