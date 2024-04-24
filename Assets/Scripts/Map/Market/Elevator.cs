using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Animator _animatorR;
    [SerializeField] private Animator _animatorL;
    [SerializeField] private Animator _otherAnimator;
    private float _random;
    private float _time;

    void Update()
    {
        _time += Time.deltaTime;

        if (_time > 5f)
        {
            _animatorR.SetBool("IsOpen", false);
            _animatorL.SetBool("IsOpen", false);
            _time = 0;

            // ¥Ÿ∏• √˛ ø§∑π∫£¿Ã≈Õ∞° ¥›«Ù¿÷¿ª ∂ß
            if(_otherAnimator.GetBool("IsOpen") == false)
            {
                _random = Random.Range(0f, 1f);
                if (_random < 0.3f)
                {
                    _animatorR.SetBool("IsOpen", true);
                    _animatorL.SetBool("IsOpen", true);
                }
            }
        }
        
    }
}
