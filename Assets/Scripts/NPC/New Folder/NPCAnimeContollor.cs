using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>애니메이션이 랜덤으로 실행되는 NPC를 실행, 중지 명령을 하는 스크립트</summary>
public class NPCAnimeContollor : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator _animator;

    [Space]
    [Header("Settings")]
    [Tooltip("애니메이터에서 컨트롤할 파라미터 이름")]
    [SerializeField] private string _parametorName;

    [Tooltip("애니메이터에서 실행할 애니메이션 번호")]
    [SerializeField] private int _animeNum;


    private Vector3 _tmpPos;

    private void OnEnable()
    {
        _animator.SetInteger(_parametorName, _animeNum);
    }

    public void Init()
    {
        _tmpPos = transform.position;
    }


    public void ShowNpc()
    {
        gameObject.SetActive(true);
        transform.position = _tmpPos;

        _animator.SetInteger(_parametorName, _animeNum);
    }


    public void HideNpc()
    {
        gameObject.SetActive(false);
    }

}
