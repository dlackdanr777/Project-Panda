using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cooking
{
    public class UICookware : MonoBehaviour
    {
        [SerializeField] private Animator[] _cookwareAnimators;


        public void CookSet()
        {
            for (int i = 0, count = _cookwareAnimators.Length; i < count; i++)
            {
                _cookwareAnimators[i].gameObject.SetActive(true);
                _cookwareAnimators[i].SetBool("Cook", false);
            }
        }


        public void CookAnimationPlay()
        {
            for (int i = 0, count = _cookwareAnimators.Length; i < count; i++)
            {
                //_cookwareAnimators[i].SetBool("Cook", true);
                _cookwareAnimators[i].SetTrigger("Cooking");
            }
        }

        public void CookStart()
        {
            for (int i = 0, count = _cookwareAnimators.Length; i < count; i++)
            {
                _cookwareAnimators[i].gameObject.SetActive(true);
                _cookwareAnimators[i].SetBool("Cook", true);
            }
        }


        public void CookEnd()
        {
            for (int i = 0, count = _cookwareAnimators.Length; i < count; i++)
            {
                _cookwareAnimators[i].SetBool("Cook", false);
            }
        }
    }
}

