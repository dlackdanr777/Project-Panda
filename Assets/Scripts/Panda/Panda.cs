using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>NPC 판다, 스타터 판다의 부모 클래스</summary>
public abstract class Panda : MonoBehaviour
{
    /// <summary>성향</summary>
    protected string _mbtiData;

    /// <summary>친밀도</summary>
    protected int _intimacy;

    /// <summary>행복도</summary>
    protected int _happiness;

    //아래에 성향 관련, 친밀도 관련 함수를 추상함수로 작성하시면 됩니다.
}
