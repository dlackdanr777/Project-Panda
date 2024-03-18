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

    [SerializeField] private bool _isConversation; // 대화 중인지 확인

    void Start()
    {
        _isConversation = false;
        _animator = GetComponent<Animator>();
        ChangeAnimation();

    }

    void Update()
    {
        if (_isConversation) // 대화 중인 경우 애니메이션 중지
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
        // 현재 맵의 애니메이션 번호 찾기
        if (_map == "MN01")
        {
            nums = _treeMapAnimations;
        }
        else
        {
            nums = _elseMapAnimations;
        }

        // 애니메이션 중 랜덤 선택
        Num = UnityEngine.Random.Range(0, nums.Length);

        _animator.SetInteger("Num", nums[Num]); // 현재 맵의 _num번째 애니메이션 실행
    }

    private void StopAnimation()
    {
        _animator.speed = 0f;
    }
}
