using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSetting : MonoBehaviour
{
    [Header("NPC�� ��Ÿ���� �ð�")]
    [SerializeField] private bool _day;
    //[SerializeField] private bool _evening;
    [SerializeField] private bool _night;

    private ETime _eTime;

    [Tooltip("�ִϸ��̼� ����")]
    [SerializeField] private int _animationCount = 1;

    private Animator _animator;
    private int _num;
    private string _map;

    [SerializeField] private bool _isConversation; // ��ȭ ������ Ȯ��

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
            if (_isConversation) // ��ȭ ���� ��� �ִϸ��̼� ����
            {
                StopAnimation();
            }
            else// �ִϸ��̼��� ���� ���� ��� ���� ����
            {
                _animator.speed = 1f;
                if (_animationCount > 1 && _map != TimeManager.Instance.CurrentMap)
                {
                    // ���� �ٲ� �� �ִϸ��̼� ����
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
