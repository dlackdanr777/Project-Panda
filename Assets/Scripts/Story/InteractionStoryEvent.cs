using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Tooltip("��ȭ ���̿� �̺�Ʈ�� �����ϴ� Ŭ����")]

public abstract class InteractionStoryEvent : StoryEvent
{
    [SerializeField] protected FollowButton _followButtonPrefab;
    private FollowButton _followButton;
    public FollowButton FollowButton => _followButton;

    [SerializeField] private Sprite _buttonImage;

    [Tooltip("��ư�� ���� Ŭ���� ������Ʈ���� �󸶸�ŭ �������־�� �ϴ���")]
    [SerializeField] private Vector3 _buttonPos;

    protected Action _nextActionHandler;

    protected virtual void Start()
    {
        Vector3 targetPos = transform.position + _buttonPos;
        Transform parent = GameObject.Find("Story Event Follow Button Parent").transform;
        _followButton = Instantiate(_followButtonPrefab, transform.position + Vector3.up, Quaternion.identity, parent);
        _followButton.Init(gameObject, targetPos, new Vector2(120, 120), _buttonImage, OnFollowButtonClicked);
        HideFollowButton();
    }


    protected abstract void OnFollowButtonClicked();


    protected void ShowFollowButton()
    {
        _followButton?.gameObject.SetActive(true);
    }

    protected void HideFollowButton()
    {
        _followButton?.gameObject.SetActive(false);
    }

}
