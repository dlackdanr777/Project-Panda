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


        public void CookPlayAnime(RecipeData data, float fireValue)
        {
            SetFoodSprite(data, fireValue);

            for (int i = 0, count = _cookAnimators.Length; i < count; i++)
            {
                _cookAnimators[i].Play("Current", -1, 0);
                _cookAnimators[i].SetTrigger("Cooking");
            }

            for (int i = 0, count = _cookwareAnimators.Length; i < count; i++)
            {
                _cookwareAnimators[i].Animator.Play("Current", -1, 0);
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


        public void SetFoodSprite(RecipeData data, float fireValue)
        {
            fireValue *= 0.01f;

            float successRangeS_x = data.SuccessRangeLevel_S;
            float successRangeA_x = successRangeS_x + data.SuccessRangeLevel_A;
            float successRangeB_x = successRangeA_x +  data.SuccessRangeLevel_B;

            float frashRange = data.SuccessLocation - successRangeB_x * 0.5f;
            float goodRange = data.SuccessLocation - (successRangeA_x * 0.5f) + (successRangeS_x * 0.5f);
            float burntRange = data.SuccessLocation + successRangeB_x * 0.5f;

            bool checkFrash = fireValue < goodRange;
            bool checkGood = goodRange <= fireValue;
            bool checkBurnt = burntRange <= fireValue;


            if (checkBurnt)
            {
                for (int i = 0, count = _cookAnimators.Length; i < count; i++)
                {
                    _cookAnimators[i].SetInteger("Rank", 2);
                }
            }

            else if (checkGood)
            {
                for (int i = 0, count = _cookAnimators.Length; i < count; i++)
                {
                    _cookAnimators[i].SetInteger("Rank", 1);
                }
            }


            else if (checkFrash)
            {
                for (int i = 0, count = _cookAnimators.Length; i < count; i++)
                {
                    _cookAnimators[i].SetInteger("Rank", 0);
                }
            }

        }
    }
}

