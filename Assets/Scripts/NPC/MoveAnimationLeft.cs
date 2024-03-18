using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimationLeft : MonoBehaviour
{
    public float speed = 0.05f;
    public Transform LeftBound;
    public Transform RightBound;
    [SerializeField] private string _animationName;
    private float _leftBound; // ���� ���
    private float _rightBound; // ������ ���
    private Animator _animator;

    private AnimatorStateInfo _stateInfo;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _leftBound = LeftBound.position.x;
        _rightBound = RightBound.position.x;
    }

    void Update()
    {
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        
        if (_stateInfo.IsName(_animationName) && _animator.speed == 1f)
        {
            transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
            if (transform.position.x < _leftBound)
            {
                transform.position = new Vector3(_rightBound, transform.position.y, transform.position.z);
            }
        }
    }
}
