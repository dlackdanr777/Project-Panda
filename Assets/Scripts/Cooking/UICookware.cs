using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cooking
{
    public class UICookware : MonoBehaviour
    {
        [SerializeField] private Animator[] _cookAnimators;

        [SerializeField] private CookwareAnimatorData[] _cookwareAnimators;


        public void CookSet()
        {
            for (int i = 0, count = _cookAnimators.Length; i < count; i++)
            {
                _cookAnimators[i].Play("Current", -1, 0);
            }

            for (int i = 0, count = _cookwareAnimators.Length; i < count; i++)
            {
                bool setActive = _cookwareAnimators[i].StartSetActive;
                _cookwareAnimators[i].gameObject.SetActive(setActive);
                _cookwareAnimators[i].Animator.Play("Current", -1, 0);

            }
        }


        public void CookAnimationPlay()
        {
            for (int i = 0, count = _cookAnimators.Length; i < count; i++)
            {
                _cookAnimators[i].SetTrigger("Cooking");
                _cookwareAnimators[i].Animator.SetTrigger("Cooking");
            }

            for (int i = 0, count = _cookwareAnimators.Length; i < count; i++)
            {
                _cookwareAnimators[i].Animator.SetTrigger("Cooking");
            }
        }

        public void CookStart()
        {
            for (int i = 0, count = _cookwareAnimators.Length; i < count; i++)
            {
                _cookwareAnimators[i].gameObject.SetActive(true);
                _cookwareAnimators[i].Animator.Play("Current", -1, 0);
            }
        }


        public void CookEnd()
        {
            for (int i = 0, count = _cookAnimators.Length; i < count; i++)
            {
                _cookAnimators[i].Play("Current", -1, 0);
            }

            for (int i = 0, count = _cookwareAnimators.Length; i < count; i++)
            {
                bool setActive = _cookwareAnimators[i].StartSetActive;
                _cookwareAnimators[i].gameObject.SetActive(setActive);
                _cookwareAnimators[i].Animator.Play("Current", -1, 0);
            }
        }
    }
}

