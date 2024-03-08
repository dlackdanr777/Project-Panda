using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSetting : MonoBehaviour
{
    [Header("NPC가 나타나는 시간")]
    [SerializeField] private bool _day;
    //[SerializeField] private bool _evening;
    [SerializeField] private bool _night;

    private ETime _eTime;
    private float _time;

    [Tooltip("애니메이션 개수")]
    [SerializeField] private int _animationCount = 1;

    private Animator _animator;
    private int _num;

    private bool _isConversation; // 대화 중인지 확인


    void Start()
    {
        _time = 0f;
        _isConversation = false;
        _eTime = TimeManager.Instance.ETime;
        SetNPC();
        _animator = GetComponent<Animator>();
        if(_animationCount > 1)
        {
            ChangeAnimation();
        }
    }

    void Update()
    {
        _time += Time.deltaTime;
        if (_eTime != TimeManager.Instance.ETime)
        {
            _eTime = TimeManager.Instance.ETime;
            SetNPC();
        }
        else if (_animationCount > 1 && _time > 10f)
        {
            _time = 0f;
            ChangeAnimation();
        }

    }

    private void SetNPC()
    {
        if (_eTime == ETime.Day)
        {
            gameObject.SetActive(_day);
        }
        //else if (_eTime == ETime.Evening)
        //{
        //    gameObject.SetActive(_evening);
        //}
        else if (_eTime == ETime.Night)
        {
            gameObject.SetActive(_night);
        }
    }

    private void ChangeAnimation()
    {
        _num = Random.Range(0, _animationCount);
        _animator.SetInteger("Num", _num);
    }
}
