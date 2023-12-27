using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.Tween;
using BT;
using System;

public class CollectionButton : MonoBehaviour, IInteraction
{
    private Animator _collectionAnim;
    [SerializeField] private GameObject _speechBubble;

    public Action<float> OnCollectionButtonClicked;
    public bool IsCollection = false; // 채집 중인가?
    private float _fadeTime = 1f; // 화면 어두운 시간
    private Vector3 _targetPos;

    private Vector3 CollectionPosition = new Vector3(-3.4f, -14f, 0);

    private void Start()
    {
        _collectionAnim = DatabaseManager.Instance.StartPandaInfo.StarterPanda.GetComponent<Animator>();
    }

    public void StartInteraction()
    {
        // 화면 FadeOut
        OnCollectionButtonClicked?.Invoke(_fadeTime);
        Invoke("ClickCollectionButton", _fadeTime);
    }

    public void UpdateInteraction()
    {

    }

    public void ExitInteraction()
    {

    }

    private void ClickCollectionButton()
    {
        StarterPanda starterPanda = DatabaseManager.Instance.StartPandaInfo.StarterPanda;

        // 캐릭터가 채집 포인트로 이동
        starterPanda.gameObject.transform.position = CollectionPosition;

        // 카메라가 캐릭터가 중앙으로 고정되도록 이동
        _targetPos = new Vector3(starterPanda.transform.position.x, starterPanda.transform.position.y, Camera.main.transform.position.z);
        Camera.main.gameObject.transform.position = _targetPos;

        IsCollection = true;
        GetComponent<SpriteRenderer>().enabled = false;

        // 화면 켜지는 시간에 맞추어 채집 시작
        Invoke("StartCollection", _fadeTime);

        
    }

    /// <summary>
    /// 카메라 움직이지 못하게 설정 </summary>
    public void CameraLock()
    {
        Camera.main.gameObject.transform.position = _targetPos;
    }

    /// <summary>
    /// 채집 시작할 때 실행 </summary>
    private void StartCollection()
    {
        Debug.Log("채집시작함니다 ~~");
        // 채집 애니메이션 판다와 말풍선 실행
        _collectionAnim.enabled = true;
        _collectionAnim.SetBool("IsCollecting", true);
        _speechBubble.GetComponent<Animator>().enabled = true;
        // 말풍선 ...이 끝나면 느낌표 표시
        // 느낌표 터치하면 채집 성공 실패 여부 알려줌
        // 애니메이션 실행 + 텍스트 띄워줌
        // 인벤토리와 도감으로 정보 전달

        // 채집 종료
        //IsCollection = false;
    }
}
