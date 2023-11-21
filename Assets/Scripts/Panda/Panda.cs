using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>NPC 판다, 스타터 판다의 부모 클래스</summary>
public abstract class Panda : MonoBehaviour
{
    [SerializeField]
    protected UIPanda _uiPanda;
    protected bool _isUISetActive;

    //상태 이미지 변경 액션
    public Action<int> StateHandler;
    public Action<float, float, Action> UIAlphaHandler;

    /// <summary>성향</summary>
    protected string _mbtiData;

    /// <summary>친밀도</summary>
    protected int _intimacy;

    /// <summary>행복도</summary>
    [SerializeField]
    [Range(-10, 10)] protected float _happiness;
    /// <summary>이전 행복도</summary>
    protected float _lastHappiness;

    //아래에 성향 관련, 친밀도 관련 함수를 추상함수로 작성하시면 됩니다.
    public abstract void ChangeIntimacy(int changeIntimacy);

    public abstract void SetPreference(string mbti);
}
