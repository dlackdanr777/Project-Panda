using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�ִϸ��̼��� �������� ����Ǵ� NPC�� ����, ���� ����� �ϴ� ��ũ��Ʈ</summary>
public class NPCAnimeContollor : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator _animator;

    [Space]
    [Header("Settings")]
    [Tooltip("�ִϸ����Ϳ��� ��Ʈ���� �Ķ���� �̸�")]
    [SerializeField] private string _parametorName;

    [Tooltip("�ִϸ����Ϳ��� ������ �ִϸ��̼� ��ȣ")]
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
