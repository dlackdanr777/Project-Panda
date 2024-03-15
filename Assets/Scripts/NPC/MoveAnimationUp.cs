using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimationUp : MonoBehaviour
{
    public float speed = 0.05f;
    public Transform MinBound;
    public Transform MaxBound;
    private float _minBound; // 위 쪽 경계
    private float _maxBound; // 아래쪽 경계
    private Animator _animator;

    private AnimatorStateInfo _stateInfo;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _minBound = MinBound.position.y;
        _maxBound = MaxBound.position.y;
    }

    void Update()
    {
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        
        if (_stateInfo.IsName("ClimbingTree_7") && _animator.speed == 1f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
            if (transform.position.y > _maxBound)
            {
                transform.position = new Vector3(transform.position.x, _minBound, transform.position.z);
            }
        }
        else if (_stateInfo.IsName("Falling_7") && _animator.speed == 1f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed * 3, transform.position.z);
            if (transform.position.y < _minBound)
            {
                transform.position = new Vector3(transform.position.x, _minBound, transform.position.z);
            }
        }
    }
}
