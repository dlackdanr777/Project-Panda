using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveAnimation : MonoBehaviour
{
    public float speed = 0.05f;
    [SerializeField] private string _leftAnimationName;
    [SerializeField] private string _upAnimationName;
    [SerializeField] private string _downAnimationName;
    private float _leftBound; // 왼쪽 경계
    private float _rightBound; // 오른쪽 경계
    private float _minBound;
    private float _maxBound; 
    private Animator _animator;

    private CameraController _cameraController;

    private string _map;

    private AnimatorStateInfo _stateInfo;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        ChangedMap();
    }

    void Update()
    {
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if(_map != TimeManager.Instance.CurrentMap)
        {
            ChangedMap();
        }
        if (_stateInfo.IsName(_leftAnimationName) && _animator.speed == 1f)
        {
            MoveLeftRight();
        }
        else if (_stateInfo.IsName(_upAnimationName) && _animator.speed == 1f)
        {
            MoveUp();
        }
        else if (_stateInfo.IsName(_downAnimationName) && _animator.speed == 1f)
        {
            MoveDown();
        }
    }

    private void MoveLeftRight()
    {
        transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
        if (transform.position.x < _leftBound || transform.position.x > _rightBound)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            speed = -speed;
        }
    }

    private void MoveUp()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
        if (transform.position.y > _maxBound)
        {
            transform.position = new Vector3(transform.position.x, _minBound, transform.position.z);
        }
    }

    private void MoveDown()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - speed * 3, transform.position.z);
        if (transform.position.y < _minBound)
        {
            transform.position = new Vector3(transform.position.x, _minBound, transform.position.z);
        }
    }

    private void ChangedMap()
    {
        _cameraController = Camera.main.GetComponent<CameraController>();

        _leftBound = _cameraController.MapCenter.x - _cameraController.MapSize.x + 5;
        _rightBound = _cameraController.MapCenter.x + _cameraController.MapSize.x - 5;
        _minBound = _cameraController.MapCenter.y - _cameraController.MapSize.y + 2.5f;
        _maxBound = _cameraController.MapCenter.y + _cameraController.MapSize.y - 2.5f;
    }
}