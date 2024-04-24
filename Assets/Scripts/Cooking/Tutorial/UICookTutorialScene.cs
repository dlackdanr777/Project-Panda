using UnityEngine;
using UnityEngine.UI;

namespace CookingTutorial
{
    public enum CookTutorialStep
    {
        Step1, Step2, Step3, Step4, Step5, Step6, Step7, Step8, Step9, Step10
    }


    public class UICookTutorialScene : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private UIMiniDialogue _dialogue;
        public UIMiniDialogue Dialogue => _dialogue;
        [SerializeField] private Image _backgroundImage;

        [Space]
        [SerializeField] private UICookTutorialStep[] _steps;

        public void Awake()
        {
            _dialogue.Init();

            for(int i = 0, count =  _steps.Length; i < count; i++)
            {
                _steps[i].Init(ChangeBackgroundImage);
            }
        }

        public void ChangeStep(CookTutorialStep step)
        {
            for(int i = 0, count = _steps.Length;i < count;i++)
            {
                _steps[i].Hide();
            }

            _steps[(int)step].Show();
        }

        private void ChangeBackgroundImage(Sprite sprite)
        {
            _backgroundImage.sprite = sprite;
        }
    }
}


