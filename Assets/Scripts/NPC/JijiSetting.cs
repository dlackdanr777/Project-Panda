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

    [System.Serializable]
    public struct MapAnimationData
    {
        public string key;
        public Transform PandaTransform;
    }

    public List<MapAnimationData> _mapAnimationDic;

    [SerializeField] private bool _isConversation; // ��ȭ ������ Ȯ��


    void Start()
    {
        _isConversation = false;
        _animator = GetComponent<Animator>();
        //ChangeAnimation();

    }

    void Update()
    {
        //if (_isConversation) // ��ȭ ���� ��� �ִϸ��̼� ����
        //{
        //    StopAnimation();
        //}
        //else
        //{
        //    _animator.speed = 1f;
        //    if (_map != TimeManager.Instance.CurrentMap)
        //    {
        //        _map = TimeManager.Instance.CurrentMap;
        //        ChangeAnimation();
        //    }
        //}
    }

    //private void ChangeAnimation()
    //{
    //    _animator.speed = 1f;
    //    _animator.Play("Idle");

    //    int[] nums;
    //    // ���� ���� �ִϸ��̼� ��ȣ ã��
    //    if (_map == "MN01")
    //    {
    //        nums = _treeMapAnimations;
    //    }
    //    else
    //    {
    //        nums = _elseMapAnimations;
    //    }

    //    // �ִϸ��̼� �� ���� ����
    //    Num = UnityEngine.Random.Range(0, nums.Length);

    //    // �ִϸ��̼ǿ� �´� ��ġ ����
    //    SetPosition();

    //    _animator.SetInteger("Num", nums[Num]); // ���� ���� _num��° �ִϸ��̼� ����
    //}

    //private void StopAnimation()
    //{
    //    _animator.speed = 0f;
    //}

    //private void SetPosition()
    //{
    //    gameObject.transform.position = _mapAnimationDic.Find(x => x.key == _map).PandaTransform.position;
    //}
}
