using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;


public class UICookingFood : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private Image _foodImage;

    [SerializeField] private Sprite _frashFood;

    [SerializeField] private Sprite _nomalFood;

    [SerializeField] private Sprite _goodFood;

    [SerializeField] private Sprite _slightlyBurntFood;

    [SerializeField] private Sprite _burntFood;

    private int _steps;

    private void Start()
    {
        _animator.enabled = false;
    }

    public void ResetSprite()
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        _foodImage.sprite = _frashFood;
    }


    public void SetFoodSprite(RecipeData data, float fireValue)
    {
        fireValue *= 0.01f;
        bool checkFrash = fireValue < data.SuccessLocation - (data.SuccessLocation * data.SuccessRangeLevel_B);
        bool checkNomal = data.SuccessLocation - (data.SuccessLocation * data.SuccessRangeLevel_B) < fireValue;
        bool checkGood = data.SuccessLocation - (data.SuccessLocation * data.SuccessRangeLevel_A) < fireValue;
        bool checkSlightlyBurnt = data.SuccessLocation + (data.SuccessLocation * data.SuccessRangeLevel_A) < fireValue;
        bool checkBurnt = data.SuccessLocation + (data.SuccessLocation * data.SuccessRangeLevel_B) < fireValue;

        if (checkBurnt)
        {
            _foodImage.sprite = _burntFood;
            _steps = 4;
        }


        else if (checkSlightlyBurnt)
        {
            _foodImage.sprite = _slightlyBurntFood;
            _steps = 3;
        }


        else if (checkGood)
        {
            _foodImage.sprite = _goodFood;
            _steps = 2;
        }


        else if (checkNomal)
        {
            _foodImage.sprite = _nomalFood;
            _steps = 1;
        }


        else if (checkFrash)
        {
            _foodImage.sprite = _frashFood;
            _steps = 0;
        }

    }


    public void StartAnime()
    {
        _animator.enabled = true;
        _animator.SetInteger("Step", _steps);
        _animator.SetTrigger("Flip");
    }
}
