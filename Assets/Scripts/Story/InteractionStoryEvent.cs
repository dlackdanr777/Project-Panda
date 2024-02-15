using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Tooltip("대화 사이에 이벤트를 실행하는 클래스")]

public abstract class InteractionStoryEvent : StoryEvent
{

    [SerializeField] private Sprite _buttonImage;

    [Tooltip("버튼이 현재 클래스 오브젝트에서 얼마만큼 떨어져있어야 하는지")]
    [SerializeField] private Vector3 _buttonPos;

    private FollowButton _followButton;
    public FollowButton FollowButton => _followButton;

    protected Action _nextActionHandler;

    protected virtual void Start()
    {
        Transform parent = GameObject.Find("Story Event Follow Button Parent").transform;
        FollowButton followButtonPrefab = Resources.Load<FollowButton>("Button/Follow Button");

        _followButton = Instantiate(followButtonPrefab, transform.position + Vector3.up, Quaternion.identity, parent);
        _followButton.Init(transform, _buttonPos, new Vector2(120, 120), _buttonImage, OnFollowButtonClicked);
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
