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

    //[System.Serializable]
    //public struct MapAnimationData
    //{
    //    public string key;
    //    public Transform PandaTransform;
    //}

    //public List<MapAnimationData> _mapAnimationDic;

    [Tooltip("판다 크기 키울 맵 ID")]
    [SerializeField] private List<string> _largeMapID = new List<string>();
    private Vector3 _pandaScale;
    private Vector3 _pandaLargeScale;

    [SerializeField] private bool _isConversation; // 대화 중인지 확인


    void Start()
    {
        _isConversation = false;
        _animator = GetComponent<Animator>();
        _map = TimeManager.Instance.CurrentMap;
        //ChangeAnimation();

        _pandaScale = gameObject.transform.localScale;
        _pandaLargeScale = gameObject.transform.localScale * 2f;

    }

    void Update()
    {
        //if (_isConversation) // 대화 중인 경우 애니메이션 중지
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

        if (_map != TimeManager.Instance.CurrentMap)
        {
            _map = TimeManager.Instance.CurrentMap;
            SetPandaSize();
        }
    }

    //private void ChangeAnimation()
    //{
    //    _animator.speed = 1f;
    //    _animator.Play("Idle");

    //    int[] nums;
    //    // 현재 맵의 애니메이션 번호 찾기
    //    if (_map == "MN01")
    //    {
    //        nums = _treeMapAnimations;
    //    }
    //    else
    //    {
    //        nums = _elseMapAnimations;
    //    }

    //    // 애니메이션 중 랜덤 선택
    //    Num = UnityEngine.Random.Range(0, nums.Length);

    //    // 애니메이션에 맞는 위치 설정
    //    SetPosition();

    //    _animator.SetInteger("Num", nums[Num]); // 현재 맵의 _num번째 애니메이션 실행
    //}

    //private void StopAnimation()
    //{
    //    _animator.speed = 0f;
    //}

    //private void SetPosition()
    //{
    //    gameObject.transform.position = _mapAnimationDic.Find(x => x.key == _map).PandaTransform.position;
    //}

    private void SetPandaSize()
    {
        if (_largeMapID.Contains(_map))
        {
            gameObject.transform.localScale = _pandaLargeScale;
        }
        else
        {
            gameObject.transform.localScale = _pandaScale;
        }
    }
}
