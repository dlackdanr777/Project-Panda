using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JijiSetting : MonoBehaviour
{

    private string _map;
    private Animator _animator;
    private int Num;

    public int[] _treeMapAnimations;
    public int[] _elseMapAnimations;

    [SerializeField] private bool _isConversation; // ��ȭ ������ Ȯ��

    void Start()
    {
        _isConversation = false;
        _animator = GetComponent<Animator>();
        ChangeAnimation();

    }

    void Update()
    {
        if (_isConversation) // ��ȭ ���� ��� �ִϸ��̼� ����
        {
            StopAnimation();
        }
        else
        {
            _animator.speed = 1f;
            if (_map != TimeManager.Instance.CurrentMap)
            {
                _map = TimeManager.Instance.CurrentMap;
                ChangeAnimation();
            }
        }
    }

    private void ChangeAnimation()
    {
        _animator.speed = 1f;

        int[] nums;
        // ���� ���� �ִϸ��̼� ��ȣ ã��
        if (_map == "MN01")
        {
            nums = _treeMapAnimations;
        }
        else
        {
            nums = _elseMapAnimations;
        }

        // �ִϸ��̼� �� ���� ����
        Num = UnityEngine.Random.Range(0, nums.Length);

        _animator.SetInteger("Num", nums[Num]); // ���� ���� _num��° �ִϸ��̼� ����
    }

    private void StopAnimation()
    {
        _animator.speed = 0f;
    }
}
