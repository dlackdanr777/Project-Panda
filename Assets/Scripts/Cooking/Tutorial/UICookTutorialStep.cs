using Muks.Tween;
using System;
using UnityEngine;

namespace CookingTutorial
{ 

    public class UICookTutorialStep : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Sprite _backgroundSprite;
        [SerializeField] private GameObject[] _arrowImages;

        private Action<Sprite> _onChagneStepHandler;

        public void Init(Action<Sprite> onChagneStepHandler)
        {
            _onChagneStepHandler = onChagneStepHandler;

            for(int i = 0, count = _arrowImages.Length; i < count; i++)
            {
                Vector3 targetPos = _arrowImages[i].transform.position + new Vector3(0, 50, 0);
                Tween.TransformMove(_arrowImages[i], targetPos, 1, TweenMode.Smoothstep).Loop(LoopType.Yoyo);
            }

            Hide();
        }


        public void Show()
        {
            _onChagneStepHandler?.Invoke(_backgroundSprite);

            for (int i = 0, count = _arrowImages.Length; i < count; i++)
            {
                _arrowImages[i].gameObject.SetActive(true);
            }
        }


        public void Hide()
        {
            for (int i = 0, count = _arrowImages.Length; i < count; i++)
            {
                _arrowImages[i].gameObject.SetActive(false);
            }
        }
    }
}
