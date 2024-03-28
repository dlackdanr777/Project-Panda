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

    [Tooltip("애니메이션 개수")]
    [SerializeField] private int _animationCount = 1;

    private Animator _animator;
    private int _num;
    private string _map;

    [SerializeField] private bool _isConversation; // 대화 중인지 확인

    private void Awake()
    {
        MainStoryController.OnFinishStoryHandler += CheckSetting;
        TimeManager.OnChangedTimeHandler += CheckTime;
    }
    private void Start()
    {
        _isConversation = false;
        _eTime = TimeManager.Instance.ETime;
        SetNPC();
        _animator = GetComponent<Animator>();
        _map = TimeManager.Instance.CurrentMap;
        if(_animationCount > 1)
        {
            ChangeAnimation();
        }
    }

    private void OnDestroy()
    {
        MainStoryController.OnFinishStoryHandler -= CheckSetting;
        TimeManager.OnChangedTimeHandler -= CheckTime;
    }

    private void CheckTime()
    {
        _eTime = TimeManager.Instance.ETime;
        SetNPC();
    }

    private void CheckSetting()
    {
        if (_animator != null)
        {
            if (_isConversation) // 대화 중인 경우 애니메이션 중지
            {
                StopAnimation();
            }
            else// 애니메이션이 여러 개인 경우 랜덤 변경
            {
                _animator.speed = 1f;
                if (_animationCount > 1 && _map != TimeManager.Instance.CurrentMap)
                {
                    // 맵이 바뀔 때 애니메이션 변경
                    _map = TimeManager.Instance.CurrentMap;
                    ChangeAnimation();
                }

            }
        }
    }

    private void SetNPC()
    {
        if (_eTime == ETime.Day)
        {
            gameObject.SetActive(_day);
        }
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

    private void StopAnimation()
    {
        _animator.speed = 0f;
    }
}
